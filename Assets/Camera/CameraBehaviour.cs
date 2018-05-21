using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour {

	public static CameraBehaviour main;

	public float moveSpeed;
	
	void Awake () {
		main = this;
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
	}

	public void SetPosition(Vector2 position) {
		Vector3 npos = position;
		npos.z = transform.position.z;
		transform.position = npos;
	}
}
