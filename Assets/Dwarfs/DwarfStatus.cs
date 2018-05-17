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

	private float mIdleHardness;
	private bool mIsSleeping;
	private float mLastCheckSleep;
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
			mFatigue += mIdleHardness * Time.deltaTime;			
		} else if(mBehaviour.activeTask != null) {
			mFatigue -= mBehaviour.activeTask.hardness * Time.deltaTime;
		} else {
			mFatigue -= mIdleHardness * Time.deltaTime;
		}

		if(mFatigue < 10f) {
			Task t = mBehaviour.taskList.Find(x => x.action == Task.Action.Sleep);
			if(t == null) mBehaviour.EnqueueTask( new SleepTask(mBed.gameObject) );
			mLastCheckSleep = Time.time;
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
