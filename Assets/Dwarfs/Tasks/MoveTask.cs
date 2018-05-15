using UnityEngine;

public class MoveTask : Task {
	
	public MoveTask(Vector3 position) : base(Task.Action.Move) {
		this.position = position;
	}

	public override string ToString() {
		return "Move :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		if(simple) return "Move";
		else return "Move to " + this.position;
	}

	public override bool Check(DwarfInventory inventory) {
		return true;
	}

	public override void Start() {

	}

	public override void Perform(DwarfInventory inventory) {

	}

	public override void Cancel() {

	}

}