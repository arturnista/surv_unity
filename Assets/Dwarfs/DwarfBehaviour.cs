using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class DwarfBehaviour : MonoBehaviour {

	public float moveSpeed = 10f;

	private GameController mGameController;
	private DwarfInventory mInventory;
	private DwarfStatus mStatus;

	private Queue<Task> mTaskQueue;
	private Task mCurrentTask;
	private List<Node> mCurrentPath;
	private bool mIsFindingPath;
	private bool mIsPerformingAction;
	private List<int> mTasksBlackList;

	public Task activeTask {
		get {
			return mCurrentTask;
		}
	}

	public List<Task> taskList {
		get {
			return mTaskQueue.ToList();
		}
	}

	public List<Node> path {
		get {
			return mCurrentPath;
		}
	}

	public DwarfInventory inventory {
		get {
			return mInventory;
		}
	}

	public DwarfStatus status {
		get {
			return mStatus;
		}
	}

	void Awake () {
		mInventory = GetComponent<DwarfInventory>();
		mStatus = GetComponent<DwarfStatus>();
		mTaskQueue = new Queue<Task>();
		mTasksBlackList = new List<int>();
	}

	void Start () {
		mGameController = GameController.main;
	}
	
	void Update () {
		if(mIsFindingPath) {
			return;
		}

		if(mIsPerformingAction) {
			return;
		}

		// If there's no active task
		if(mCurrentTask == null) {
			// If there's active task to be done
			if(mTaskQueue.Count > 0 || mGameController.taskList.Count > 0) {
				// Get the next task
				mCurrentTask = GetNextTask();
				// if(mCurrentTask != null) mCurrentPath = Pathfinder.main.FindPath(transform.position, mCurrentTask.position);
				if(mCurrentTask != null) {
					mIsFindingPath = true;
					Pathfinder.main.FindPathAsync(transform.position, mCurrentTask.position, (path) => {
						mIsFindingPath = false;
						mCurrentPath = path;
						if(path == null) HUDController.main.CreateFloatingText("Not possible to move here", mCurrentTask.position, Color.red);
					});
				}
				return;
			}
			return;
		}
		
		if(mCurrentPath == null) {
			mCurrentTask = null;
			return;
		}
		if(mCurrentPath.Count == 0) {
			PerformTask();
			return;
		}

		Node node = mCurrentPath[0];

		if(node == null) {
			HUDController.main.CreateFloatingText("Not possible to " + mCurrentTask.ToStringFormat(), mCurrentTask.position, Color.red);
			mCurrentTask.Cancel();

			mCurrentTask = null;
			mCurrentPath = null;

			return;
		}

		transform.position = Vector3.MoveTowards(transform.position, node.worldPosition, moveSpeed * Time.deltaTime);
		if(Vector2.Distance(transform.position, node.worldPosition) <= 0.1f) {
			mCurrentPath.RemoveAt(0);
		}
	}

	bool SideStep() {
		List<Node> nodes = Pathfinder.main.GetAvailableNeighbours(transform.position);
		if(nodes.Count == 0) return false;

		transform.position = nodes[0].worldPosition;
		return true;
	}

	public void EnqueueTask(Task task) {
		if(!CheckTask(task)) {
			HUDController.main.CreateFloatingText("Not possible", task.position, Color.red);
			return;
		}

		HUDController.main.CreateFloatingText(task.ToStringFormat(), task.position, Color.white);

		mTaskQueue.Enqueue(task);

		task.Start();
	}

	public void ClearAndEnqueueTask(Task task) {
		if(!CheckTask(task)) return;

		ClearTasks();		
		mTaskQueue.Enqueue(task);

		task.Start();
	}

	void PerformTask() {
		if(!CheckTask(mCurrentTask)) {
			mCurrentTask.Cancel();
			mCurrentTask = null;
			return;
		}
		if(Vector2.Distance(transform.position, mCurrentTask.position) > 1.5f) {
			mCurrentTask.Cancel();
			mCurrentTask = null;
			return;
		}

		if(mCurrentTask.action == Task.Action.Construct) {
			if(!SideStep()) {
				mCurrentTask.Cancel();
				mCurrentTask = null;
			}
		}

		mIsPerformingAction = true;
		mCurrentTask.Perform(mInventory, () => {
			mIsPerformingAction = false;
			mCurrentTask = null;
		});
	}

	bool CheckTask(Task task) {
		if(!task.Check(mInventory)) return false;
		// Node node = Pathfinder.main.FindNextNode(transform.position, task.position);		

		// if(node == null) return false;
		return true;
	}

	Task GetNextTask(int idx = 0) {
		Task task;
		if(mTaskQueue.Count > 0) {
			task = mTaskQueue.Dequeue();
			if(!task.Check(mInventory)) return GetNextTask();
		} else if (idx < mGameController.taskList.Count) {
			task = mGameController.taskList[idx];
			if(mTasksBlackList.Contains( task.id )) return null;
			
			if(!task.Check(mInventory)) {
				mTasksBlackList.Add( task.id );
				return GetNextTask(idx + 1);
			}
			mGameController.taskList.RemoveAt(idx);
		} else {
			return null;
		}

		return task;
	}

	void ClearTasks() {
		while(mTaskQueue.Count > 0) {
			Task task = mTaskQueue.Dequeue();
			task.Cancel();
		}
		if(mCurrentTask != null) mCurrentTask.Cancel();
		mTaskQueue.Clear();		
	}

}
