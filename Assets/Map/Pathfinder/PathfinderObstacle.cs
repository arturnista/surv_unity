using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderObstacle : MonoBehaviour {

	public bool isStatic = true;

	protected Vector2Int mPosition;
	protected Transform mPathfinderCollider;

	protected virtual void Awake () {
		mPosition = General.ToVector2Int(transform.position);
		if(mPathfinderCollider == null) mPathfinderCollider = transform.Find("Collider");
        mPathfinderCollider.gameObject.layer = LayerMask.NameToLayer("PathFinderCollider");
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
