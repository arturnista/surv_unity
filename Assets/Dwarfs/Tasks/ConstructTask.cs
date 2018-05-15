using UnityEngine;

public class ConstructBuildingTask : Task {

	public Building building;
	protected GameBuilding mBuildingGhost;
	
	public ConstructBuildingTask(Vector3 position, Building building) : base(Task.Action.ConstructBuilding) {
		this.position = position;
        this.building = building;
	}

	public override string ToString() {
		return "ConstructBuilding :: " + building.name + " :: " + this.position;
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

	public override void Perform(DwarfInventory inventory) {
        building.DestroyGhost(mBuildingGhost);
        building.CreateGameObject(position);
        foreach(Building.Requirement req in building.requirements) {
            inventory.RemoveItem(req.item, req.amount);
        }
	}

	public override void Cancel() {
        if(mBuildingGhost != null) building.DestroyGhost(mBuildingGhost);
	}

}