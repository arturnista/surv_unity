using UnityEngine;

public class Task {
	public enum Action {
		None,
		Move,
		CutTree,
		PickUpItem,
		Construct,
		Deconstruct,
	}

	protected static int iLastID = 0;

	public int id;
	public float hardness;
	public Vector3 position;
	public GameObject target;
	public Action action;
	protected System.Action mOnFinishCallback;
	protected DwarfInventory mDwarfInventory;

	void CreateID() {
		id = iLastID++;
	}

	public Task(Action action) {
		CreateID();
		this.action = action;
		this.hardness = 1f;
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

	public virtual void Perform(DwarfInventory inventory, System.Action onFinish) {
		mDwarfInventory = inventory;
		mOnFinishCallback = onFinish;
	}

	protected virtual void FinishPerform() {

	}

	public virtual void Cancel() {

	}
	
}