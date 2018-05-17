using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutTreeTask : Task {

	private Tree mTree;
	
	public CutTreeTask(GameObject target) : base(Task.Action.CutTree) {
		this.target = target;
		this.position = target.transform.position;
		this.hardness = 1f;

		mTree = target.GetComponent<Tree>();
	}

	public override string ToString() {
		return "CutTree :: " + target.name + " :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		return "Cut tree";
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
        yield return new WaitForSeconds(mTree.cutTime);

		List<GameItem> itemsCreated = mTree.Cut();
		foreach(GameItem t in itemsCreated) {
			GameController.main.PushTask( new PickUpItemTask(t.gameObject) );
		}

		mOnFinishCallback();
	}

	public override void Cancel() {

	}

}