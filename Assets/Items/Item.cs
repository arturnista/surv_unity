using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/Create Item")]
public class Item : ScriptableObject {

	public new string name;
	public Sprite sprite;
	public GameObject prefab;

	public enum Type {
		Resource,
		Food,
		Weapon
	}
	public Type[] type;

	public GameItem CreateGameObject(Vector3 position) {
		return CreateGameObject(position, Quaternion.identity);
	}

	public GameItem CreateGameObject(Vector3 position, Quaternion rotation) {
		position.z = -10f;
		GameItem gItem = Instantiate(prefab, position, rotation).GetComponent<GameItem>();
		gItem.Configure(this);

		return gItem;
	}

}
