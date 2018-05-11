using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem {
	public InventoryItem(int index, Item item) {
		this.index = index;
		this.item = item;
		amount = 1;
	}
	public int index;
	public Item item;
	public int amount;
}

public class DwarfInventory : MonoBehaviour {

	private HUDController mHUDController;

	private List<InventoryItem> mInventory;
	private int mLastIndex;

	public List<InventoryItem> inventory {
		get {
			return mInventory;
		}
	}

	void Awake () {
		mInventory = new List<InventoryItem>();
		mLastIndex = 0;
	}
	
	void Start () {
		mHUDController = HUDController.main;
	}

	public bool CheckItem(Item item, int amount) {
		InventoryItem invItem = mInventory.Find(x => x.item == item);
		if(invItem == null) return false;
		if(invItem.amount < amount) return false;

		return true;
	}

	public void AddItem(Item item, int amount) {
		InventoryItem invItem = mInventory.Find(x => x.item == item);
		if(invItem == null) mInventory.Add(new InventoryItem(mLastIndex++, item));
		else invItem.amount += amount;

		mHUDController.UpdateInventory();
	}

	public void RemoveItem(Item item, int amount) {
		InventoryItem invItem = mInventory.Find(x => x.item == item);
		if(invItem == null) return;

		invItem.amount -= amount;
		if(invItem.amount <= 0) mInventory.Remove(invItem);

		mHUDController.UpdateInventory();
	}
}
