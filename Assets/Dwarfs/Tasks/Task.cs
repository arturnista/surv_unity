using System.Collections;
using UnityEngine;

public class Task {
	public enum Action {
		None,
		Move,
        ResourceGather,
		PickUpItem,
		Construct,
		Deconstruct,
		Sleep,
	}

	protected static int iLastID = 0;

	public int id;
	public float hardness;
	public Vector3 position;
	public GameObject target;
	public Action action;
	protected System.Action mOnFinishCallback;
	protected DwarfInventory mDwarfInventory;
	protected DwarfStatus mDwarfStatus;
	protected DwarfBehaviour mDwarfBehaviour;

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
		mDwarfStatus = inventory.GetComponent<DwarfStatus>();
		mDwarfBehaviour = inventory.GetComponent<DwarfBehaviour>();
		mOnFinishCallback = onFinish;
	}

	protected virtual IEnumerator FinishPerform() {
		yield return null;
	}

	public virtual void Cancel() {

	}
	
}