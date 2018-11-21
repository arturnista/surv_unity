using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGather : MonoBehaviour {
	
	public float gatherTime = 2f;
    public string resourceAction = "Cut tree";

    [SerializeField]
	private Item mItem;
	[SerializeField]
	private int mItemCount = 10;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public List<GameItem> Gather() {
		List<GameItem> ret = new List<GameItem>();
		for (int i = 0; i < mItemCount; i++) {
			Vector3 pos = transform.position;
			pos.x += Random.Range(-1f, 1f);
			pos.y += Random.Range(-1f, 1f);
			GameItem it = mItem.CreateGameObject(pos, Quaternion.identity);	
			ret.Add(it);
		}

		Destroy(gameObject);

		return ret;
	}

}
