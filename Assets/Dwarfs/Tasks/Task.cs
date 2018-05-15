using UnityEngine;

public class Task {
	public enum Action {
		None,
		Move,
		CutTree,
		PickUpItem,
		ConstructBuilding
	}

	protected static int iLastID = 0;

	public int id;
	public Vector3 position;
	public GameObject target;
	public Action action;

	void CreateID() {
		id = iLastID++;
	}

	public Task(Action action) {
		CreateID();
	}

	public override string ToString() {
		return action.ToString();
	}

	public virtual string ToStringFormat(bool simple = false) {
		return action.ToString();
	}

	public virtual bool Check(DwarfInventory inventory) {
		return false;
	}

	public virtual void Start() {

	}

	public virtual void Perform(DwarfInventory inventory) {

	}

	public virtual void Cancel() {

	}
	
}