using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class ActionTaker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

	private SpriteRenderer mSpriteRenderer;
	private GameArea mGameArea;
	private List<Task> mTasks;
	private int mMoveActionIndex;

	private Vector3 mInitialPointerDown;
	private bool mIsSelected;

	private bool mShouldResetQueue;

	void Awake () {
		mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		mTasks = new List<Task>();

		Tree tree = GetComponent<Tree>();
		if(tree) mTasks.Add( new Task(Task.Action.CutTree, gameObject) );
		
		GameItem item = GetComponent<GameItem>();
		if(item) mTasks.Add( new Task(Task.Action.PickUpItem, gameObject) );	

		mGameArea = GetComponent<GameArea>();
		mTasks.Add( new Task(Task.Action.Move, gameObject) );
		mMoveActionIndex = mTasks.Count - 1;

		mShouldResetQueue = false;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			mShouldResetQueue = true;
		} else if(Input.GetKeyUp(KeyCode.LeftShift)) {
			mShouldResetQueue = false;
		}
	}

	void Select() {
		mIsSelected = true;
		if(mSpriteRenderer) mSpriteRenderer.color = new Color(.3f, .3f, .3f, 1f);
	}

	void Deselect() {
		mIsSelected = false;
		if(mSpriteRenderer) mSpriteRenderer.color = new Color(1f, 1f, 1f, 1f);		
	}

    public void OnPointerDown(PointerEventData eventData) {
		if (!EventSystem.current.IsPointerOverGameObject()) return;

		mInitialPointerDown = eventData.position;
	}

    public void OnPointerUp(PointerEventData eventData) {
		if (!EventSystem.current.IsPointerOverGameObject()) return;

		if(Vector3.Distance(eventData.position, mInitialPointerDown) >= 32f) {
			return;
		}

		// If its and game area object
		if(mGameArea != null) {

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0f;

			// If the game area has an active building, must construct it
			if(GameController.main.activeBuilding != null) {
				Vector3Int mousePosInt = new Vector3Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);
				Task constructTask = new Task(Task.Action.ConstructBuilding, mousePosInt, GameController.main.activeBuilding);
				DispatchTask(constructTask);
				return;
			}

			// Create the move task based on where the player clicked
			mTasks[mMoveActionIndex] = new Task(Task.Action.Move, mousePos);
		}

		if (eventData.button == PointerEventData.InputButton.Left) {
		
			// Create the list of actions to perform
			List<string> act = new List<string>();
			foreach (Task t in mTasks) act.Add(Task.ActionToString(t.action));
			// act.Add("Select all");
			HUDController.main.OpenActionCanvas(Input.mousePosition, act, SelectAction);

		} else if (eventData.button == PointerEventData.InputButton.Right) {

			// If clicked with the right button, perfom the first action
			HUDController.main.CloseActionCanvas();
			DispatchTask(mTasks[0]);
		
		}
    }

	public void OnPointerEnter(PointerEventData pointerEventData) {
		if (!EventSystem.current.IsPointerOverGameObject()) return;
		Select();
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
		if (!EventSystem.current.IsPointerOverGameObject()) return;
		Deselect();
    }

	void SelectAction(int index) {
		if(index == -1) return;
		if(index == mTasks.Count) {
			Debug.Log("Select all");
			return;
		}

		DispatchTask(mTasks[index]);
	}
	
	void DispatchTask(Task task) {
		if(!mShouldResetQueue) GameController.main.DispatchTask(task);
		else GameController.main.ClearAndDispatchTask(task);
	}
}
