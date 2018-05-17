using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour {

	public float cutTime = 2f;

	[SerializeField]
	private Item mWoodLog;
	private int mLogCount = 10;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<GameItem> Cut() {
		List<GameItem> ret = new List<GameItem>();
		for (int i = 0; i < mLogCount; i++) {
			Vector3 pos = transform.position;
			pos.x += Random.Range(-1f, 1f);
			pos.y += Random.Range(-1f, 1f);
			GameItem it = mWoodLog.CreateGameObject(pos, Quaternion.identity);	
			ret.Add(it);
		}

		Destroy(gameObject);

		return ret;
	}
}
