using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class GameArea : MonoBehaviour {

	public static GameArea main;

	private BoxCollider2D mGameAreaCollider;

	void Awake () {
		mGameAreaCollider = GetComponent<BoxCollider2D>();
		Vector3 p = transform.position;
		p.z = 1f;
		transform.position = p;

		main = this;
	}

	void Start () {
		UpdateGameArea();
	}

	public void UpdateGameArea() {
        float vSize = Camera.main.orthographicSize * 4f;
        float hSize = Camera.main.aspect * vSize;
        mGameAreaCollider.size = new Vector2(hSize, vSize);
	}

}
