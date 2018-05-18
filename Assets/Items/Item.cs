using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Create Item")]
public class Item : ScriptableObject {

	public new string name;
	public Sprite sprite;
	public GameObject prefab;

	public enum ItemType {
		Resource,
		Food,
		Weapon
	}
	public List<ItemType> types;

	public GameItem CreateGameObject(Vector3 position) {
		return CreateGameObject(position, Quaternion.identity);
	}

	public GameItem CreateGameObject(Vector3 position, Quaternion rotation) {
		position.z = -10f;
		GameItem gItem = Instantiate(prefab, position, rotation).GetComponent<GameItem>();
		gItem.Configure(this);

		return gItem;
	}

	public virtual bool IsType(ItemType type) {
		ItemType t = types.Find(x => x == type);
		return t == type;
	}

}
