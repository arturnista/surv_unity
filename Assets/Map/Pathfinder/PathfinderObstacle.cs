using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderObstacle : MonoBehaviour {

	public bool isStatic = true;

	private Vector2Int mPosition;
	private Transform mPathfinderCollider;

	void Awake () {
		mPosition = General.ToVector2Int(transform.position);
		mPathfinderCollider = transform.Find("Collider");
	}

	void Update() {
		if(!isStatic) return;
		
		if(mPosition != General.ToVector2Int(transform.position)) {
			mPosition = General.ToVector2Int(transform.position);
			Pathfinder.main.RefreshWalkable(mPosition);
		}
	}
	
	void OnDisable() {
		if(Pathfinder.main == null) return;

		mPathfinderCollider.gameObject.layer = LayerMask.NameToLayer("Default");
		Pathfinder.main.RefreshWalkable(mPosition);
	}

	void OnEnable() {
		if(Pathfinder.main == null) return;

		Pathfinder.main.RefreshWalkable(mPosition);		
	}

}
