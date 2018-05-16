using UnityEngine;

public class PickUpItemTask : Task {
	
	private GameItem mGameItem;

	public PickUpItemTask(GameObject target) : base(Task.Action.PickUpItem) {
		this.target = target;
		this.position = target.transform.position;
		mGameItem = target.GetComponent<GameItem>();
	}		


	public override string ToString() {
		return "PickUpItem :: " + target.name + " :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		if(simple) return "Pick up";
		return "Pick up " + mGameItem.item.name;
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
        Item item = mGameItem.PickUp();
        inventory.AddItem(item, 1);

		onFinish();
	}

	public override void Cancel() {

	}

}