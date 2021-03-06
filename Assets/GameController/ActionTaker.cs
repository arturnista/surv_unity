﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(BoxCollider2D))]
public class ActionTaker : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

	private SpriteRenderer mSpriteRenderer;
	private GameArea mGameArea;
	private List<Task> mTasks;
	private int mMoveActionIndex;

	private Vector3 mLastPosition;

	private Vector3 mInitialPointerDown;
	private bool mIsSelecting;
	private List<ActionTaker> mActionsSelected;
	private bool mIsSelected;

	private bool mShouldResetQueue;

	public Task defaultTask {
		get {
			return mTasks[0];
		}
	}

	void Awake () {
		mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		mTasks = new List<Task>();
		mActionsSelected = new List<ActionTaker>();

		mShouldResetQueue = false;
	}

	void Start() {

        ResourceGather resource = GetComponent<ResourceGather>();
		if(resource) mTasks.Add( new ResourceGatherTask(gameObject) );
		
		GameItem item = GetComponent<GameItem>();
		if(item) mTasks.Add( new PickUpItemTask(gameObject) );	
		
		Bed bed = GetComponent<Bed>();
		if(bed) mTasks.Add( new SleepTask(gameObject) );	
		
		GameBuilding building = GetComponent<GameBuilding>();
		if(building) mTasks.Add( new DeconstructTask(gameObject) );	

		mGameArea = GetComponent<GameArea>();
		mTasks.Add( new MoveTask(transform.position) );
		mMoveActionIndex = mTasks.Count - 1;
		
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			mShouldResetQueue = true;
		} else if(Input.GetKeyUp(KeyCode.LeftShift)) {
			mShouldResetQueue = false;
		}
		Vector3 fPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(mIsSelecting && mLastPosition != fPos) {
			mLastPosition = fPos;
			foreach(ActionTaker a in mActionsSelected) a.Deselect();
			mActionsSelected.Clear();

			Vector3 iPos = Camera.main.ScreenToWorldPoint(mInitialPointerDown);
			Collider2D[] colls = Physics2D.OverlapAreaAll(iPos, fPos);
			foreach(Collider2D c in colls) {
				ActionTaker action = c.GetComponent<ActionTaker>();
				if(action && action != this && !mActionsSelected.Contains(action)) {
					mActionsSelected.Add(action);
					action.Select();
				}
			}
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
		HUDController.main.StartSelecting();
		mIsSelecting = true;
	}

    public void OnPointerUp(PointerEventData eventData) {
		if (!EventSystem.current.IsPointerOverGameObject()) return;

		HUDController.main.StopSelecting();
		mIsSelecting = false;
		foreach(ActionTaker a in mActionsSelected) a.Deselect();
		mActionsSelected.Clear();

		if(Vector3.Distance(eventData.position, mInitialPointerDown) >= 32f) {
			Vector3 iPos = Camera.main.ScreenToWorldPoint(mInitialPointerDown);
			Vector3 fPos = Camera.main.ScreenToWorldPoint(eventData.position);
			Collider2D[] colls = Physics2D.OverlapAreaAll(iPos, fPos);
			foreach(Collider2D c in colls) {
				ActionTaker action = c.GetComponent<ActionTaker>();
				if(action && action != this) EnqueueTask( action.defaultTask );	
			}
			return;
		}

		// If its and game area object
		if(mGameArea != null) {

			Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mousePos.z = 0f;

			// If the game area has an active building, must construct it
			if(GameController.main.activeBuilding != null) {
				Vector3Int mousePosInt = new Vector3Int(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);
				Task constructTask = new ConstructTask(mousePosInt, GameController.main.activeBuilding);
				EnqueueTask(constructTask);
				return;
			}

			// Create the move task based on where the player clicked
			mTasks[mMoveActionIndex] = new MoveTask(mousePos);
		}

		if (eventData.button == PointerEventData.InputButton.Left) {
		
			// Create the list of actions to perform
			List<string> act = new List<string>();
			foreach (Task t in mTasks) act.Add(t.ToStringFormat(true));
			// act.Add("Select all");
			HUDController.main.OpenActionCanvas(Input.mousePosition, act, SelectAction);

		} else if (eventData.button == PointerEventData.InputButton.Right) {

			// If clicked with the right button, perfom the first action
			HUDController.main.CloseActionCanvas();
			EnqueueTask(mTasks[0]);
		
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

		EnqueueTask(mTasks[index]);
	}
	
	void EnqueueTask(Task task) {
		if(!mShouldResetQueue) GameController.main.EnqueueTask(task);
		else GameController.main.ClearAndEnqueueTask(task);
	}
}
