using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Pathfinder : MonoBehaviour {

	public static Pathfinder main;

	public LayerMask pathfinderMask;
	
	private Vector2Int mGridSize = new Vector2Int(400, 400);
	private Dictionary<Vector2Int, Node> mGrid;

	private Vector2 mStartPosAsync;
	private Vector2 mTargetPosAsync;
	private bool mFindNeighbour;
	private Action<List<Node>> mCallbackAsync;

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

	public List<Node> GetAvailableNeighbours(Vector2 position) {
		Node n = NodeFromPosition(position);
		List<Node> nei = FindNeighbours(n);
		return nei.FindAll(x => x.walkable);
	}

	Node NodeFromPosition(Vector2 position) {
		return mGrid[General.ToVector2Int(position)];
	}

	List<Node> FindNeighbours(Node node) {
		List<Node> neighbours = new List<Node>();
		for (int x = -node.size.x; x <= node.size.x; x += node.size.x) {
			for (int y = -node.size.y; y <= node.size.y; y += node.size.y) {
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

		if(startNode == targetNode) return new List<Node>( new Node[] { targetNode } );
		if(!startNode.walkable) return null;
		if(!targetNode.walkable) return null;

		// Add the first node to the set
		Heap<Node> openSet = new Heap<Node>(gridTotalSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		// While open set is not empty
		while(openSet.Count != 0) {
			Node currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			// Found the path
			if(currentNode == targetNode) {
				return ReversePath(startNode, targetNode);
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

		// Add
		// Heap<Node> openSetReversed = new Heap<Node>(gridTotalSize);
		// HashSet<Node> closedSetReversed = new HashSet<Node>();
		// openSetReversed.Add(targetNode);

		// while(openSet.Count != 0 && openSetReversed.Count != 0) {
		// 	UnityEditor.EditorApplication.isPaused = true;
		// 	Node node = FindNode(targetNode, openSet, closedSet);

		// 	Node nodeRev = FindNode(startNode, openSetReversed, closedSetReversed);

		// 	if(openSetReversed.Contains(node)) {
		// 		Debug.Log("ACHOU");
		// 		return null;
		// 	} else if(openSet.Contains(nodeRev)) {
		// 		Debug.Log("ACHOU");
		// 		return null;
		// 	} else if(node == targetNode) {
		// 		Debug.Log("ACHOU");
		// 		return null;
		// 	} else if(nodeRev == startNode) {
		// 		Debug.Log("ACHOU");
		// 		return null;
		// 	}
		// }

		Debug.Log("NAO ACHOU");
		return null;
	}

	public void FindPathAsync(Vector2 startPos, Vector2 targetPos, Action<List<Node>> callback) {
		FindPathAsync(startPos, targetPos, false, callback);
	}

	public void FindPathAsync(Vector2 startPos, Vector2 targetPos, bool neighbour, Action<List<Node>> callback) {
		mStartPosAsync = startPos;
		mTargetPosAsync = targetPos;
		mCallbackAsync = callback;
		mFindNeighbour = neighbour;

		StartCoroutine(FindPathCoroutine());
	}

	IEnumerator FindPathCoroutine() {
		Node startNode = NodeFromPosition(mStartPosAsync);
		Node targetNode = NodeFromPosition(mTargetPosAsync);

		if(startNode == targetNode) {
			mCallbackAsync(new List<Node>( new Node[] { targetNode } ));
			yield break;
		}
		if(!startNode.walkable) {
			mCallbackAsync(null);
			yield break;
		}
		if(!targetNode.walkable) {
			mFindNeighbour = true;
		}
		// Add the first node to the set
		Heap<Node> openSet = new Heap<Node>(gridTotalSize);
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		float lastTime;
		int frames = 0;

		// While open set is not empty
		while(openSet.Count != 0) {
			lastTime = Time.time;
			Node currentNode = openSet.RemoveFirst();

			closedSet.Add(currentNode);

			// Found the path
			if(currentNode == targetNode) {
				List<Node> path = ReversePath(startNode, targetNode);
				mCallbackAsync(path);
				yield break;
			}

			// Loop thru all the neighours
			List<Node> neighbours = FindNeighbours(currentNode);
			foreach (Node neighbour in neighbours) {

				if(mFindNeighbour && neighbour == targetNode) {
					List<Node> path = ReversePath(startNode, currentNode);
					mCallbackAsync(path);
					yield break;
				}

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

			if(++frames > 500) {
				// lastTime = Time.time;
				frames = 0;
				yield return null;
			}
		}

		mCallbackAsync(null);
		yield break;
	}

	List<Node> ReversePath(Node startNode, Node targetNode) {
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

	// void OnDrawGizmos() {
	// 	Gizmos.DrawWireCube(transform.position, General.ToVector3(mGridSize));
	// 	if(mGrid != null) {
	// 		foreach(KeyValuePair<Vector2Int, Node> entry in mGrid) {
	// 			if(!entry.Value.walkable) Gizmos.color = Color.red;
	// 			else Gizmos.color = Color.white;
	// 			Gizmos.DrawWireCube(entry.Value.worldPosition, General.ToVector3(entry.Value.size));
	// 		}
	// 	}
	// }

}
