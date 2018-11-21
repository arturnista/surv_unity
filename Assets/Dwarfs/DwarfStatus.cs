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
	public float healthPerc {
		get {
			float perc = mHealth / maxHealth;
			if(perc < 0) return 0;
			return perc;
		}
	}
	public float fatiguePerc {
		get {
			float perc = mFatigue / maxFatigue;
			if(perc < 0) return 0;
			return perc;
		}
	}
	public float hungryPerc {
		get {
			float perc = mHungry / maxHungry;
			if(perc < 0) return 0;
			return perc;
		}
	}
	public string dwarfName {
		get { return "Dwarf name"; }
	}

	private float mIdleHardness = 1f;
	private bool mIsSleeping;
	private bool mAlreadyEnqueueSleepTask;
    private bool mWaitingForBed;
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
	private DwarfInventory mInventory;

	void Awake () {
		mBehaviour = GetComponent<DwarfBehaviour>();
		mInventory = GetComponent<DwarfInventory>();
	}

	void Start () {
		mHealth = maxHealth;
		mHungry = maxHungry;
		mFatigue = maxFatigue;
		// mIdleHardness = 1f;
	}
	
	void Update () {
		if(mIsSleeping) {
			mFatigue += mBed.restness * Time.deltaTime;			
		} else if(mBehaviour.activeTask != null) {
			mFatigue -= mBehaviour.activeTask.hardness * Time.deltaTime;
		} else {
			mFatigue -= mIdleHardness * Time.deltaTime;
		}

		if(fatiguePerc < .1f) {
			if(!mWaitingForBed && mBed == null) {
                mBed = GameObject.FindObjectOfType<Bed>();
				if(mBed == null) {
                	mWaitingForBed = true;
					HUDController.main.CreateFloatingText("We need more beds!", Vector3.zero, Color.yellow);
					EventManager.AddEventListener("OnBedConstruct", HandleBedConstruct);
				}
			}
			if(mBed != null && !mAlreadyEnqueueSleepTask) {
                mWaitingForBed = false;
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

		if(hungryPerc < .1f) {
			ConsumeFood();
		}

	}

	public void StartSleep(Bed bed) {
		mBed = bed;
		mIsSleeping = true;
	}

	public void StopSleep() {
		mIsSleeping = false;
	}

	void HandleBedConstruct() {
		if(!mWaitingForBed) return;

        mWaitingForBed = false;
    	EventManager.RemoveEventListener("OnBedConstruct", HandleBedConstruct);
	}

	public void ConsumeFood() {
		List<InventoryItem> foods = mInventory.inventory.FindAll(x => x.item.IsType(Item.ItemType.Food) );
		while(foods.Count > 0 && hungryPerc < .8f) {
			ItemFood iFood = (ItemFood) foods[0].item;
			int amountConsumed = 0;
			for (int i = 0; i < foods[0].amount; i++) {
				mHungry += iFood.Consume();
				amountConsumed++;
				if(hungryPerc >= .8f) break;
			}

			mInventory.RemoveItem(iFood, amountConsumed);
			foods.RemoveAt(0);
		}
	}

}
