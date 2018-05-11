using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IHeapItem<Node> {

	public bool walkable;
	public Vector2Int position;
	public Vector3 worldPosition {
        get {
            return new Vector3(position.x, position.y);
        }
    }
	public int gCost;
	public int hCost;
    public Node parent;
	public int fCost {
		get {
			return gCost + hCost;
		}
	}

	private int mHeapIndex;
	public int HeapIndex {
		get {
			return mHeapIndex;
		}

		set {
			mHeapIndex = value;
		}
	}

	private Vector2 mSize;

	public int CompareTo(Node a) {
		int compare = fCost.CompareTo(a.fCost);
		if(compare == 0) {
			compare = hCost.CompareTo(a.hCost);
		}

		return -compare;
	}

    private LayerMask mPathfinderMask;

	public Node(int x, int y, LayerMask pathfinderMask) {
		this.position = new Vector2Int(x, y);
        this.mPathfinderMask = pathfinderMask;
        this.RefreshWalkable();
		mSize = Vector2.one / 2f;
	}

    public void RefreshWalkable() {
		Collider2D collider = Physics2D.OverlapBox(this.position, mSize, 0f, mPathfinderMask);
		this.walkable = collider == null;
    }

    public override string ToString() {
        return "Node: " + position.ToString();
    }
}