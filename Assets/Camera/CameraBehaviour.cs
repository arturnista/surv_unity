using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public static CameraBehaviour main;

	private PixelPerfectCamera pixelPerfect;
	private GameArea gameArea;
	public float moveSpeed;
	
	void Awake () {
		main = this;
        pixelPerfect = GetComponent<PixelPerfectCamera>();
        gameArea = GetComponentInChildren<GameArea>();
	}
	
	void Update () {
		if(Input.GetKey(KeyCode.W)) {
			transform.Translate(moveSpeed * Vector3.up * Time.deltaTime);
		} else if(Input.GetKey(KeyCode.S)) {
			transform.Translate(moveSpeed * Vector3.down * Time.deltaTime);
		}

		if(Input.GetKey(KeyCode.D)) {
			transform.Translate(moveSpeed * Vector3.right * Time.deltaTime);
		} else if(Input.GetKey(KeyCode.A)) {
			transform.Translate(moveSpeed * Vector3.left * Time.deltaTime);
		}

		if(Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus)) {
			pixelPerfect.IncreaseZoom();
			gameArea.UpdateGameArea();
		} else if(Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus)) {
			pixelPerfect.DecreaseZoom();
			gameArea.UpdateGameArea();
		}
	}

	public void SetPosition(Vector2 position) {
		Vector3 npos = position;
		npos.z = transform.position.z;
		transform.position = npos;
	}
}
