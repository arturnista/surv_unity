using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameItem : MonoBehaviour {

	public Item item;
	private SpriteRenderer mSpriteRenderer;
	
	void Awake () {
		mSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
		General.GroupGameObjects(transform, "Items");
		if(this.item) Configure(this.item);
	}

	public void Configure(Item item) {
		this.item = item;
		
		name = item.name + " - " + name;
		mSpriteRenderer.sprite = item.sprite;
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Item PickUp() {
		Destroy(gameObject);

		return item;
	}
}
