using UnityEngine;

public class MoveTask : Task {
	
	public MoveTask(Vector3 position) : base(Task.Action.Move) {
		this.position = position;
		this.hardness = 1f;
	}

	public override string ToString() {
		return "Move :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		return "Move";
	}

	public override bool Check(DwarfInventory inventory) {
		return true;
	}

	public override void Start() {

	}

	public override void Perform(DwarfInventory inventory, System.Action onFinish) {
		onFinish();
	}
	
	public override void Cancel() {

	}

}