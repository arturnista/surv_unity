﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class DwarfBehaviour : MonoBehaviour {

	public float moveSpeed = 10f;

	private GameController mGameController;
	private DwarfInventory mInventory;

	private Queue<Task> mTaskQueue;
	private Task mCurrentTask;
	private List<Node> mCurrentPath;
	private bool mIsFindingPath;
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

	void Awake () {
		mInventory = GetComponent<DwarfInventory>();
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

		// If there's no active task
		if(mCurrentTask == null) {
			// If there's active task to be done
			if(mTaskQueue.Count > 0 || mGameController.taskList.Count > 0) {
				// Get the next task
				mCurrentTask = GetNextTask();
				// if(mCurrentTask != null) mCurrentPath = Pathfinder.main.FindPath(transform.position, mCurrentTask.targetPosition);
				if(mCurrentTask != null) {
					mIsFindingPath = true;
					Pathfinder.main.FindPathAsync(transform.position, mCurrentTask.targetPosition, (path) => {
						mIsFindingPath = false;
						mCurrentPath = path;
						if(path == null) HUDController.main.CreateFloatingText("Not possible here", mCurrentTask.targetPosition, Color.red);
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
			HUDController.main.CreateFloatingText("Not possible to " + Task.ActionToString(mCurrentTask.action), mCurrentTask.targetPosition, Color.red);
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

	public void EnqueueTask(Task task) {
		if(!CheckTask(task)) {
			HUDController.main.CreateFloatingText("Not possible", task.targetPosition, Color.red);
			return;
		}

		HUDController.main.CreateFloatingText(Task.ActionToString(task.action), task.targetPosition, Color.white);

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
			mCurrentTask = null;
			return;
		}
		if(Vector2.Distance(transform.position, mCurrentTask.targetPosition) > 1f) {
			mCurrentTask = null;
			return;
		}

		mCurrentTask.Perform(mInventory);
		mCurrentTask = null;
	}

	bool CheckTask(Task task) {
		if(!task.Check(mInventory)) return false;
		Node node = Pathfinder.main.FindNextNode(transform.position, task.targetPosition);		

		if(node == null) return false;
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
