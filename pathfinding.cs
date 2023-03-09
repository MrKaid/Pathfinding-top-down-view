
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pathfinding
{

    //more efficient to move diagonally than straight
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static pathfinding Instance { get; private set; }

    private myGrid<node> grid;
    private List<node> openList;
    private List<node> closedList;

    public pathfinding(int width, int height, LayerMask layer)
    {
        Instance = this;
        grid = new myGrid<node>(width, height, .724f, Vector3.zero, (myGrid<node> g, int x, int y) => new node(g, x, y));
        CheckMap(width, height, layer);
    }

    public Vector3 GetPositionOfNode(int x, int y)
    {
        return grid.GetWorldPosition(x, y) + new Vector3(.362f, .362f, 0);
    }

    public void CheckMap(int width, int height, LayerMask layer)
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Collider2D collider2D = Physics2D.OverlapCircle(grid.GetWorldPosition(x, y) + new Vector3(.362f, .362f, 0), .724f / 5, layer);

                if (collider2D != null)
                {
                    GetNode(x, y).isWalkable = false;
                }
                else
                {
                    GetNode(x, y).isWalkable = true;
                }
            }
        }
    }

    public myGrid<node> GetGrid()
    {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<node> path = FindPath(startX, startY, endX, endY);

        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (node pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.row, pathNode.col) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<node> FindPath(int startX, int startY, int endX, int endY)
    {
        node startNode = grid.GetGridObject(startX, startY);
        node endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null)
        {
            // Invalid Path
            return null;
        }

        openList = new List<node> { startNode };
        closedList = new List<node>();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                node pathNode = grid.GetGridObject(x, y);
                pathNode.g = 99999999;
                pathNode.setF();
                pathNode.parent = null;
            }
        }

        startNode.g = 0;
        startNode.h = CalculateDistanceCost(startNode, endNode);
        startNode.setF();


        while (openList.Count > 0)
        {
            node currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (node neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = currentNode.g + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.g)
                {
                    neighbourNode.parent = currentNode;
                    neighbourNode.g = tentativeGCost;
                    neighbourNode.h = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.setF();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;
    }

    //checks neighbor for a neighbor nodes and adds to the list of neighbors to consider
    private List<node> GetNeighbourList(node currentNode)
    {
        List<node> neighbourList = new List<node>();

        // Left
        if (currentNode.row - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.row - 1, currentNode.col));
        }

        // Right
        if (currentNode.row + 1 < grid.GetWidth())
        {
            neighbourList.Add(GetNode(currentNode.row + 1, currentNode.col));
        }

        // Down
        if (currentNode.col - 1 >= 0)
        {
            neighbourList.Add(GetNode(currentNode.row, currentNode.col - 1));
        }

        // Up
        if (currentNode.col + 1 < grid.GetHeight())
        {
            neighbourList.Add(GetNode(currentNode.row, currentNode.col + 1));
        }

        return neighbourList;
    }

    public node GetNode(int x, int y)
    {
        return grid.GetGridObject(x, y);
    }

    private List<node> CalculatePath(node endNode)
    {
        List<node> path = new List<node>();
        path.Add(endNode);
        node currentNode = endNode;
        while (currentNode.parent != null)
        {
            path.Add(currentNode.parent);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(node a, node b)
    {
        int xDistance = Mathf.Abs(a.row - b.row);
        int yDistance = Mathf.Abs(a.col - b.col);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //gets whichever neighbor is best move
    private node GetLowestFCostNode(List<node> pathNodeList)
    {
        node lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].f < lowestFCostNode.f)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

}
