using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	[SerializeField]
	private Item mWoodLog;
	private int mLogCount = 10;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Cut() {
		for (int i = 0; i < mLogCount; i++) {
			Vector3 pos = transform.position;
			pos.x += Random.Range(-1f, 1f);
			pos.y += Random.Range(-1f, 1f);
			mWoodLog.CreateGameObject(pos, Quaternion.identity);	
		}

		Destroy(gameObject);
	}
}
