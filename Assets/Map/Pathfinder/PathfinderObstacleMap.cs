using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfinderObstacleMap : PathfinderObstacle {

    protected override void Awake() {
        mPathfinderCollider = transform;
		base.Awake();
    }

}
