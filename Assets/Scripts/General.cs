using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General {

	public static Vector2Int ToVector2Int(Vector2 pos) {
		int x = Mathf.RoundToInt(pos.x);
		int y = Mathf.RoundToInt(pos.y);

		return new Vector2Int(x, y);
    }

	public static Vector3 ToVector3(Vector2Int pos, float z = 0f) {
		return new Vector3(pos.x, pos.y, z);
    }

	public static Vector3 ToVector3(Vector3Int pos) {
		return new Vector3(pos.x, pos.y, pos.z);
    }

	public static Vector3Int ToVector3Int(Vector3 pos) {
		int x = Mathf.RoundToInt(pos.x);
		int y = Mathf.RoundToInt(pos.y);
		int z = Mathf.RoundToInt(pos.z);

		return new Vector3Int(x, y, z);
    }

	public static void GroupGameObjects(Transform transform, string group) {
		GameObject parent = GameObject.Find(group);
		if(!parent) parent = new GameObject(group);
		transform.SetParent(parent.transform);
	}
}
