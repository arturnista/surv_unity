using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Building", menuName = "Buildings/Create building")]
public class Building : ScriptableObject {

	public new string name;
	public Vector2 size;
	public Sprite sprite;
	public float buildTime;
	public GameObject prefab;
	public GameObject ghostPrefab;
	[System.Serializable]
	public struct Requirement {
		public Item item;
		public int amount;
	}
	public List<Requirement> requirements;

	public Vector3 FixPosition(Vector3 pos) {
		Vector3 position = pos;

		position.z = -5f;
		if(size.x % 2 == 0) position.x -= .5f;
		if(size.y % 2 == 0) position.y -= .5f;

		return position;
	}

	public GameBuilding CreateGhost(Vector3 position) {
		return CreateGhost(position, 0f);
	}

	public GameBuilding CreateGhost(Vector3 position, float rotation) {
		GameBuilding ghost = Instantiate(ghostPrefab, FixPosition(position), Quaternion.Euler(0f, 0f, rotation)).GetComponent<GameBuilding>();
		ghost.Configure(this);

		return ghost;
	}

	public void DestroyGhost(GameBuilding ghost) {
		if(ghost) Destroy(ghost.gameObject);
	}

	public GameBuilding CreateGameObject(Vector3 position) {
		return CreateGameObject(position, 0f);
	}

	public GameBuilding CreateGameObject(Vector3 position, float rotation) {
		GameBuilding gBuilding = Instantiate(prefab, FixPosition(position), Quaternion.Euler(0f, 0f, rotation)).GetComponent<GameBuilding>();
		gBuilding.Configure(this);

		return gBuilding;
	}

}
