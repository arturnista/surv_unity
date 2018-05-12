using UnityEngine;

public class Task {
	public enum Action {
		None,
		Move,
		CutTree,
		PickUpItem,
		ConstructBuilding
	}

	private static int iLastID = 0;

	public int id;
	public Vector3 targetPosition;
	public GameObject target;
	public Action action;
	public Building building;
	private GameBuilding mBuildingGhost;

	public Task(Action action, GameObject target) {
		this.action = action;
		this.target = target;
		this.targetPosition = target.transform.position;

		CreateID();
	}

	public Task(Action action, Vector3 pos) {
		this.action = action;
		this.targetPosition = pos;

		CreateID();
	}

	public Task(Action action, Vector3 pos, Building building) {
		this.action = action;
		this.targetPosition = pos;
		this.building = building;

		CreateID();
	}

	void CreateID() {
		id = iLastID++;
	}

	public override string ToString() {
		return action.ToString();
	}

	public bool Check(DwarfInventory inventory) {
		switch(action) {
			case Task.Action.CutTree:
			case Task.Action.PickUpItem:
				if(target == null) {
					Cancel();
					return false;
				}
				break;
			case Task.Action.ConstructBuilding:
				foreach(Building.Requirement req in building.requirements) {
					if(!inventory.CheckItem(req.item, req.amount)) {
						Cancel();
						return false;
					}
				}
				break;
		}

		return true;
	}

	public void Start() {
		switch(action) {
			case Action.ConstructBuilding:
				mBuildingGhost = building.CreateGhost(this.targetPosition);
				break;
		}
	}

	public void Perform(DwarfInventory inventory) {
		switch(action) {
			case Task.Action.CutTree:
				target.GetComponent<Tree>().Cut();
				break;
			case Task.Action.PickUpItem:
				Item item = target.GetComponent<GameItem>().PickUp();
				inventory.AddItem(item, 1);
				break;
			case Task.Action.ConstructBuilding:
				building.DestroyGhost(mBuildingGhost);
				building.CreateGameObject(targetPosition);
				foreach(Building.Requirement req in building.requirements) {
					inventory.RemoveItem(req.item, req.amount);
				}
				break;
		}
	}

	public void Cancel() {
		switch(action) {
			case Action.ConstructBuilding:
				if(mBuildingGhost != null) building.DestroyGhost(mBuildingGhost);
				break;
		}
	}

	public static string ActionToString(Action ac) {
		switch(ac) {
			case Action.None:
				return "None";
			case Action.Move:
				return "Move";
			case Action.CutTree:
				return "Cut";
			case Action.PickUpItem:
				return "Pick Up";
			case Action.ConstructBuilding:
				return "Construct";
			default:
				return "";
		}
	}

}