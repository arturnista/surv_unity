﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeconstructTask : Task {

	public Building building;
	protected GameBuilding mGameBuilding;
	
	public DeconstructTask(GameObject gameBuilding) : base(Task.Action.Deconstruct) {
		this.position = gameBuilding.transform.position;
		this.mGameBuilding = gameBuilding.GetComponent<GameBuilding>();
		this.building = this.mGameBuilding.building;
	}

	public override string ToString() {
		return "Deconstruct :: " + mGameBuilding.name + " :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		if(simple) return "Deconstruct";
		return "Deconstruct " + mGameBuilding.name;
	}

	public override bool Check(DwarfInventory inventory) {
		return true;
	}

	public override void Start() {
		
	}

	public override void Perform(DwarfInventory inventory, System.Action onFinish) {
		base.Perform(inventory, onFinish);
		this.hardness = 3f;
		
		inventory.StartCoroutine(FinishPerform());
	}
	
	protected override IEnumerator FinishPerform() {
        yield return new WaitForSeconds(this.building.buildTime);

		List<GameItem> itemsCreated = mGameBuilding.Deconstruct();
		foreach(GameItem t in itemsCreated) {
			mDwarfBehaviour.PushTask( new PickUpItemTask(t.gameObject) );
			// GameController.main.PushTask( new PickUpItemTask(t.gameObject) );
		}
		mOnFinishCallback();
	}

	public override void Cancel() {

	}

}