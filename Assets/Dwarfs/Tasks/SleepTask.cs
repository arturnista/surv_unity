using System.Collections;
using UnityEngine;

public class SleepTask : Task {

	private Bed mBed;
	private DwarfStatus mDwarfStatus;

	public SleepTask(GameObject bed) : base(Task.Action.Sleep) {
		this.mBed = bed.GetComponent<Bed>();
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

		return true;
	}

	public override void Start() {

	}

	public override void Perform(DwarfInventory inventory, System.Action onFinish) {
		base.Perform(inventory, onFinish);

		mDwarfStatus = inventory.GetComponent<DwarfStatus>();
		mDwarfStatus.StartSleep();
		mDwarfStatus.transform.position = target.transform.position;

		// mDwarfInventory.Invoke("FinishPerfom", .3f);
		mDwarfInventory.StartCoroutine(Example());

		// System.Threading.Thread.Sleep(1000);
		
		// FinishPerform();
	}

    IEnumerator Example() {
		Debug.Log("Começou");
        yield return new WaitForSeconds(5);
		FinishPerform();
		Debug.Log("Terminou");
    }

	protected override void FinishPerform() {
		mDwarfStatus.transform.position = Pathfinder.main.GetAvailableNeighbours(mDwarfStatus.transform.position)[0].worldPosition;

		mDwarfStatus.StopSleep();
		mOnFinishCallback();
	}

	public override void Cancel() {

	}

}
