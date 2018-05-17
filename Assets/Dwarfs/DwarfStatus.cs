using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DwarfStatus : MonoBehaviour {

	public float maxHealth;
	public float maxHungry;
	public float maxFatigue;

	private float mHealth;
	private float mHungry;
	private float mFatigue;

	public float health {
		get { return mHealth; }
	}
	public float hungry {
		get { return mHungry; }
	}
	public float fatigue {
		get { return mFatigue; }
	}
	public float fatiguePerc {
		get { return mFatigue / maxFatigue; }
	}

	private float mIdleHardness;
	private bool mIsSleeping;
	private bool mAlreadyEnqueueSleepTask;
	private Bed mBed;
	public Bed bed {
		get {
			return mBed;
		}
		set {
			mBed = value;
		}
	}

	private DwarfBehaviour mBehaviour;

	void Awake () {
		mBehaviour = GetComponent<DwarfBehaviour>();
	}

	void Start () {
		mHealth = maxHealth;
		mHungry = maxHungry;
		mFatigue = maxFatigue;
		mIdleHardness = 1f;
	}
	
	void Update () {
		if(mIsSleeping) {
			mFatigue += mBed.restness * Time.deltaTime;			
		} else if(mBehaviour.activeTask != null) {
			mFatigue -= mBehaviour.activeTask.hardness * Time.deltaTime;
		} else {
			mFatigue -= mIdleHardness * Time.deltaTime;
		}

		if(mBed && fatiguePerc < .1f) {
			if(!mAlreadyEnqueueSleepTask) {
				Task t = mBehaviour.taskList.Find(x => x.action == Task.Action.Sleep);
				if(t == null) mBehaviour.EnqueueTask( new SleepTask(mBed.gameObject) );
				mAlreadyEnqueueSleepTask = true;			
			}
		} else {
			mAlreadyEnqueueSleepTask = false;			
		}
		
		if(mBehaviour.activeTask != null) {
			mHungry -= mBehaviour.activeTask.hardness * Time.deltaTime;
		} else {
			mHungry -= mIdleHardness * Time.deltaTime;
		}

	}

	public void StartSleep(Bed bed) {
		mBed = bed;
		mIsSleeping = true;
	}

	public void StopSleep() {
		mIsSleeping = false;
	}


}
