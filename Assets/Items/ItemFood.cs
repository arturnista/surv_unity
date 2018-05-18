using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Items/Create Food")]
public class ItemFood : Item {

	public float hungry;

	public float Consume() {
		return hungry;
	}

	public override bool IsType(ItemType type) {
		return type == Item.ItemType.Food;
	}

}
