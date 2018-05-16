using UnityEngine;

public class ConstructTask : Task {

	public Building building;
	protected GameBuilding mBuildingGhost;
	
	public ConstructTask(Vector3 position, Building building) : base(Task.Action.Construct) {
		this.position = position;
        this.building = building;
	}

	public override string ToString() {
		return "Construct :: " + building.name + " :: " + this.position;
	}

	public override string ToStringFormat(bool simple = false) {
		if(simple) return "Construct";
		return "Construct " + building.name;
	}

	public override bool Check(DwarfInventory inventory) {
        foreach(Building.Requirement req in building.requirements) {
            if(!inventory.CheckItem(req.item, req.amount)) {
                Cancel();
                return false;
            }
        }

		return true;
	}

	public override void Start() {
		mBuildingGhost = building.CreateGhost(this.position);
	}

	public override void Perform(DwarfInventory inventory, System.Action onFinish) {
		base.Perform(inventory, onFinish);
		this.hardness = 3f;
		
		// Invoke("FinishPerfom", this.building.buildTime);
		FinishPerform();
	}
	
	protected override void FinishPerform() {
        building.DestroyGhost(mBuildingGhost);
        building.CreateGameObject(position);
        foreach(Building.Requirement req in building.requirements) {
            mDwarfInventory.RemoveItem(req.item, req.amount);
        }

		mOnFinishCallback();
	}

	public override void Cancel() {
        if(mBuildingGhost != null) building.DestroyGhost(mBuildingGhost);
	}

}