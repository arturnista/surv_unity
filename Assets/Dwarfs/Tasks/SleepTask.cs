using System.Collections;
using UnityEngine;

public class SleepTask : Task {

	private Bed mBed;
	private GameBuilding mBuilding;

	public SleepTask(GameObject bed) : base(Task.Action.Sleep) {
		this.mBed = bed.GetComponent<Bed>();
		this.mBuilding = bed.GetComponent<GameBuilding>();
		this.target = bed;
		this.position = target.transform.position;
	}		


	public override string ToString() {
		return "Sleep :: " + target.name + " :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		return "Sleep";
	}

	public override bool Check(DwarfInventory inventory) {
        if(target == null) {
            Cancel();
            return false;
        }

		if(mBed.isOccupied) {
            Cancel();
            return false;
		}

		return true;
	}

	public override void Start() {

	}

	public override void Perform(DwarfInventory inventory, System.Action onFinish) {
		base.Perform(inventory, onFinish);
		
		mDwarfStatus.StartSleep(mBed);
		mBed.StartSleep();

		mDwarfInventory.StartCoroutine(FinishPerform());
	}

	protected override IEnumerator FinishPerform() {
		mDwarfStatus.transform.position = target.transform.position;

		while(mDwarfStatus.fatiguePerc < 0.9f) {
	        yield return new WaitForSeconds(1f);
		}

		mDwarfStatus.transform.position = Pathfinder.main.GetAvailableNeighbours(mDwarfStatus.transform.position, mBuilding.building.size)[0].worldPosition;

		mDwarfStatus.StopSleep();
		mBed.StopSleep();

		mOnFinishCallback();
	}

	public override void Cancel() {

	}

}
