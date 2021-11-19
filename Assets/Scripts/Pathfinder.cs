using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder
{

    private const int MOVE_FORWARD_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private int MAXx;
    private int MAXy;

    private List<CubeStateInfo> vmap = new List<CubeStateInfo>();

    public Pathfinder(List<CubeStateInfo> map, int MaxX, int MaxY)
    {
        MAXx = MaxX;
        MAXy = MaxY;
        vmap = map;
    }

    public List<CubeStateInfo> FindPath(CubeStateInfo start, CubeStateInfo finish)
    {
        CubeStateInfo startNode = start;
        CubeStateInfo endNode = finish;

        List<CubeStateInfo> openList = new List<CubeStateInfo> { startNode };
        List<CubeStateInfo> closedList = new List<CubeStateInfo>();

        for (int x = 0; x < MAXx; x++)
        {
            for (int y = 0; y < MAXy; y++)
            {
                CubeStateInfo pathNode = GetMapObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            CubeStateInfo currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (CubeStateInfo nearNode in GetRoundList(currentNode))
            {
                if (closedList.Contains(nearNode)) continue;
                if (!nearNode.isWalkable)
                {
                    closedList.Add(nearNode);
                    continue;
                }

                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, nearNode);
                if (tentativeGCost < nearNode.gCost)
                {
                    nearNode.cameFromNode = currentNode;
                    nearNode.gCost = tentativeGCost;
                    nearNode.hCost = CalculateDistanceCost(nearNode, endNode);
                    nearNode.CalculateFCost();

                    if (!openList.Contains(nearNode))
                    {
                        openList.Add(nearNode);
                    }
                }

            }

        }
        return new List<CubeStateInfo> { startNode };
    }

    public CubeStateInfo GetMapObject(int x, int y)
    {
        if (x >= 0 && y >= 0 && x < MAXx && y < MAXy)
        {
            return vmap[(x * MAXx) + y];
        }
        else
        {
            return default(CubeStateInfo);
        }
    }


    private List<CubeStateInfo> GetRoundList(CubeStateInfo currentNode)
    {
        List<CubeStateInfo> roundList = new List<CubeStateInfo>();

        if (currentNode.x - 1 >= 0) // Left
        {
            roundList.Add(GetMapObject(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) roundList.Add(GetMapObject(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < MAXy) roundList.Add(GetMapObject(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < MAXx) // Right
        {
            roundList.Add(GetMapObject(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) roundList.Add(GetMapObject(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < MAXy) roundList.Add(GetMapObject(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) roundList.Add(GetMapObject(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < MAXy) roundList.Add(GetMapObject(currentNode.x, currentNode.y + 1));

        return roundList;
    }

    private int CalculateDistanceCost(CubeStateInfo startNode, CubeStateInfo endNode)
    {
        int xDistance = Mathf.Abs(startNode.x - endNode.x);
        int yDistance = Mathf.Abs(startNode.y - endNode.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_FORWARD_COST * remaining;
    }

    private CubeStateInfo GetLowestFCostNode(List<CubeStateInfo> pathNodeList)
    {
        CubeStateInfo lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }
        return lowestFCostNode;
    }

    private List<CubeStateInfo> CalculatePath(CubeStateInfo endNode)
    {
        List<CubeStateInfo> path = new List<CubeStateInfo>();
        path.Add(endNode);
        CubeStateInfo currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }
}
