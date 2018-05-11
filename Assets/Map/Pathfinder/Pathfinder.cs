using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour {

	public static Pathfinder main;

	public LayerMask pathfinderMask;
	
	private Vector2Int mGridSize = new Vector2Int(400, 400);
	private Dictionary<Vector2Int, Node> mGrid;

	void OnDrawGizmos() {
		Gizmos.DrawWireCube(transform.position, General.ToVector3(mGridSize));
		// if(mGrid != null) {
		// 	foreach(KeyValuePair<Vector2Int, Node> entry in mGrid) {
		// 		if(!entry.Value.walkable) Gizmos.color = Color.red;
		// 		else Gizmos.color = Color.white;
		// 		Gizmos.DrawWireCube(entry.Value.worldPosition, Vector2.one);
		// 	}
		// }
	}

	private int gridTotalSize {
		get {
			return mGridSize.x * mGridSize.y;
		}
	}

	void Awake () {
		main = this;
		CreateGrid();
	}
	
	void CreateGrid() {
		mGrid = new Dictionary<Vector2Int, Node>();

		int halfX = mGridSize.x / 2;
		int halfY = mGridSize.y / 2;

		for (int x = -halfX; x < halfX; x++) {
			for (int y = -halfY; y < halfY; y++) {
				Vector2Int pos = new Vector2Int(x, y);
				mGrid.Add(pos, new Node(x, y, pathfinderMask));
			}
		}
	}

	Node NodeFromPosition(Vector2 position) {
		return mGrid[General.ToVector2Int(position)];
	}

	List<Node> FindNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();
		for (int x = -1; x <= 1; x++) {
			for (int y = -1; y <= 1; y++) {
				if(x == 0 && y == 0) continue;

				Vector2Int p = new Vector2Int(node.position.x + x, node.position.y + y);

				if(mGrid.ContainsKey(p)) neighbours.Add(mGrid[p]);
			}	
		}

		return neighbours;
	}

	int GetDistance(Node a, Node b) {
		int xDist = Mathf.Abs(a.position.x - b.position.x);
		int yDist = Mathf.Abs(a.position.y - b.position.y);

		if(xDist > yDist) return 14 * yDist + 10 * (xDist - yDist);
		else return 14 * xDist + 10 * (yDist - xDist);
	}

	public Node FindNextNode(Vector2 startPos, Vector2 targetPos) {
		List<Node> nodes = FindPath(startPos, targetPos);
		if(nodes == null || nodes.Count == 0) return null;
		return nodes[0];
	}

	public void RefreshWalkable(Vector2 pos) {
		Node n = mGrid[General.ToVector2Int(pos)];
		n.RefreshWalkable();

		foreach (Node nei in FindNeighbours(n)) nei.RefreshWalkable();
	}

	public List<Node> FindPath(Vector2 startPos, Vector2 targetPos) {
		Node startNode = NodeFromPosition(startPos);
		Node targetNode = NodeFromPosition(targetPos);

		Heap<Node> openSet = new Heap<Node>(gridTotalSize);
		HashSet<Node> closedSet = new HashSet<Node>();

		// Add the first node to the set
		openSet.Add(startNode);

		if(startNode == targetNode) return new List<Node>( new Node[] { targetNode } );

		// While open set is not empty
		while(openSet.Count != 0) {
			Node currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			// Found the path
			if(currentNode == targetNode) {
				return FindPath(startNode, targetNode);
			}

			// Loop thru all the neighours
			List<Node> neighbours = FindNeighbours(currentNode);
			foreach (Node neighbour in neighbours) {

				if(!neighbour.walkable || closedSet.Contains(neighbour)) continue;
				
				int newCost = currentNode.gCost + GetDistance(currentNode, neighbour);
				if(newCost < neighbour.gCost || !openSet.Contains(neighbour)) {
					neighbour.gCost = newCost;
					neighbour.hCost = GetDistance(neighbour, targetNode);
					neighbour.parent = currentNode;

					if(!openSet.Contains(neighbour)) {
						openSet.Add(neighbour);
					} else {
						openSet.UpdateItem(neighbour);
					}
				}
			}
		}

		return null;
	}

	List<Node> FindPath(Node startNode, Node targetNode) {
		// Recreates the path from the target to the start node
		List<Node> path = new List<Node>();
		Node cNode = targetNode;
		while(cNode != startNode) {
			path.Add(cNode);
			cNode = cNode.parent;
		}
		path.Reverse();

		return path;
	}

}
