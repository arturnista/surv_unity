using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static GameController main;
	
	private LineRenderer mPathRenderer;
	private DwarfBehaviour mCurrentDwarf;
	private HUDController mHUDController;

	private List<Task> mTaskList;

	private Building mActiveBuilding;
	public Building activeBuilding {
		get {
			return mActiveBuilding;
		}

		set {
			mActiveBuilding = value;
		}
	}

	public DwarfBehaviour dwarf {
		get {
			return mCurrentDwarf;
		}
		set {
			mCurrentDwarf = value;
			mHUDController.UpdateInventory();
		}
	}

	public List<Task> taskList {
		get {
			return mTaskList;
		}
	}

	void Awake () {
		main = this;
		// mCurrentDwarf = GameController.FindObjectOfType<DwarfBehaviour>();
		mPathRenderer = GetComponent<LineRenderer>();

		mTaskList = new List<Task>();
	}

	void Start () {
		mHUDController = HUDController.main;
	}
	
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape)) {
			mActiveBuilding = null;
			mCurrentDwarf = null;
		} else if(Input.GetKeyDown(KeyCode.Alpha1)) {
			mHUDController.ToggleBuildingsListMenu();
		} else if(Input.GetKeyDown(KeyCode.Alpha2)) {
			mHUDController.ToggleInventoryMenu();
		}

		if(mCurrentDwarf == null || mCurrentDwarf.path == null) return;

		List<Vector3> pos = new List<Vector3>();
		pos.Add(new Vector3(mCurrentDwarf.transform.position.x, mCurrentDwarf.transform.position.y, -1f));
		foreach(Node n in mCurrentDwarf.path) pos.Add(new Vector3(n.worldPosition.x, n.worldPosition.y, -1f));
		mPathRenderer.positionCount = pos.Count;
		mPathRenderer.SetPositions(pos.ToArray());
	}

	public void EnqueueTask(Task task) {
		if(dwarf) {
			dwarf.EnqueueTask(task);
		} else {
			HUDController.main.CreateFloatingText(task.ToStringFormat(), task.position, Color.white);
			
			mTaskList.Add(task);
			task.Start();
		}
	}

	public void PushTask(Task task) {
		if(dwarf) {
			dwarf.EnqueueTask(task);
		} else {
			HUDController.main.CreateFloatingText(task.ToStringFormat(), task.position, Color.white);
			
			mTaskList.Insert(0, task);
			task.Start();
		}
	}

	public void ClearAndEnqueueTask(Task task) {
		if(dwarf) {
			dwarf.ClearAndEnqueueTask(task);
		} else {
			ClearTasks();
			mTaskList.Add(task);
		}
	}

	void ClearTasks() {
		while(mTaskList.Count > 0) {
			Task task = mTaskList[0];
			mTaskList.RemoveAt(0);
			task.Cancel();
		}
		mTaskList.Clear();		
	}

	
}
