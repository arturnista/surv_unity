using UnityEngine;

public class CutTreeTask : Task {

	private Tree mTree;
	
	public CutTreeTask(GameObject target) : base(Task.Action.CutTree) {
		this.target = target;
		this.position = target.transform.position;
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

	public override void Perform(DwarfInventory inventory) {
		mTree.Cut();
	}

	public override void Cancel() {

	}

}