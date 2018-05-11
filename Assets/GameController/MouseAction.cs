using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAction : MonoBehaviour {

	private SpriteRenderer mSpriteRenderer;
	private Sprite mDefaultSprite;

	private Color mDefaultColor;

	void Awake () {
		mSpriteRenderer = GetComponent<SpriteRenderer>();
		mDefaultSprite = mSpriteRenderer.sprite;
		mDefaultColor = mSpriteRenderer.color;

		Debug.LogWarning("Fix this to not update in every UPDATE");
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePos.z = 0f;
		Vector3 mousePosInt = new Vector3(Mathf.RoundToInt(mousePos.x), Mathf.RoundToInt(mousePos.y), 0);

		if(transform.position != mousePosInt) {
			if(GameController.main.activeBuilding != null) {
				mSpriteRenderer.sprite = GameController.main.activeBuilding.sprite;
				mSpriteRenderer.color = new Color(1f, 1f, 1f, .3f);
				mousePosInt = GameController.main.activeBuilding.FixPosition(mousePosInt);
			} else {
				mSpriteRenderer.sprite = mDefaultSprite;
				mSpriteRenderer.color = mDefaultColor;
			}
		}
		transform.position = mousePosInt;
	}
}
