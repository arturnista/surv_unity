using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherTask : Task {

	private ResourceGather mResource;
	
	public ResourceGatherTask(GameObject target) : base(Task.Action.ResourceGather) {
		this.target = target;
		this.position = target.transform.position;
		this.hardness = 1f;

        mResource = target.GetComponent<ResourceGather>();
	}

	public override string ToString() {
		return mResource.resourceAction + " :: " + target.name + " :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		return mResource.resourceAction;
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
		this.hardness = 2f;

		inventory.StartCoroutine(FinishPerform());
	}
	
	protected override IEnumerator FinishPerform() {
        yield return new WaitForSeconds(mResource.gatherTime);

		List<GameItem> itemsCreated = mResource.Gather();
		foreach(GameItem t in itemsCreated) {
			mDwarfBehaviour.PushTask( new PickUpItemTask(t.gameObject) );
			// GameController.main.PushTask( new PickUpItemTask(t.gameObject) );
		}

		mOnFinishCallback();
	}

	public override void Cancel() {

	}

}