using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item list", menuName = "Items/Create Item list")]
public class ItemList : ScriptableObject {

	public List<Item> itemList;

}
