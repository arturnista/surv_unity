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
}
