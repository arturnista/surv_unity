using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBuilding : MonoBehaviour {

	private Building mBuilding;
	private SpriteRenderer mSpriteRenderer;

	void Awake () {
		mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		General.GroupGameObjects(transform, "Buildings");
	}

	public void Configure(Building building) {
		name = building.name + " - " + name;
		mBuilding = building;
		
		mSpriteRenderer.sprite = mBuilding.sprite;
		
		foreach(BoxCollider2D box in GetComponentsInChildren<BoxCollider2D>()) {
			box.size = building.size;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	public void Deconstruct() {

		foreach(Building.Requirement req in mBuilding.requirements) {
			for (int i = 0; i < req.amount; i++) {
				Vector3 pos = transform.position;
				pos.x += Random.Range(-1f, 1f);
				pos.y += Random.Range(-1f, 1f);
				GameItem it = req.item.CreateGameObject(pos);	
			}
		}

		Destroy(gameObject);
	}
}
