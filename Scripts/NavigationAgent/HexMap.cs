﻿using Generic.Contants;
using Generic.Singleton;
using PathFinding;
using System.Collections.Generic;
using UnityEngine;
using static NodeManagerFactory;

public sealed class HexMap : MonoSingle<HexMap>
{
    public Grid HexGrid;

    public const int TotalCol = Constants.TOTAL_COL;
    public const int TotalRow = Constants.TOTAL_ROW;

    private NodeManagerFactory managerFactory;
    public NodeManagerFactory NodeManagerFactory
    {
        get
        {
            return managerFactory ?? (managerFactory = Singleton.Instance<NodeManagerFactory>());
        }
    }

    private SingleWayPointManager agentNodeManager;
    public SingleWayPointManager AgentNodeManager
    {
        get
        {
            return agentNodeManager ??
                (agentNodeManager =
                NodeManagerFactory.GetManager<AgentWayPoint>(NodeType.Single) as SingleWayPointManager);
        }
    }
    private BreathFirstSearch breathFirstSearch;

    private void Start()
    {
        breathFirstSearch = Singleton.Instance<BreathFirstSearch>();
    }

    public int ConvertToIndex(int x, int y)
    {
        return y * TotalCol + x;
    }

    public Vector3Int ConvertToVector3Int(int index)
    {
        Vector3Int result = Vector3Int.zero;
        result.x = index % TotalCol;
        result.y = index / TotalCol;

        if (IsValidCell(result.x, result.y)) return Vector3Int.one * -1;
        return result;
    }

    public Vector3Int[] GetNeighboursRange1(Vector3Int center)
    {
        #region old
        //List<Vector3Int> neighbours = new List<Vector3Int>();
        //Vector3Int neighbour;
        //Vector3Int[] pattern = (cell.y % 2 == 0) ? HexaPatternEven1 : HexaPatternOdd1;
        //for (int i = 0; i < pattern.Length; i++)
        //{
        //    neighbour = pattern[i] + cell;
        //    if (IsValidCell(neighbour.x, neighbour.y))
        //    {
        //        neighbours.Add(neighbour);
        //    }
        //}
        //return neighbours.ToArray();
        #endregion

        return Constants.GetNeighboursRange(center, 1);
    }

    #region Not use
    public Vector3Int[] GetNeighboursEmpty(Vector3Int cell)
    {
        List<Vector3Int> neighbours = new List<Vector3Int>();
        Vector3Int neighbour;
        Vector3Int[] pattern = (cell.y % 2 == 0) ?
            Constants.Neighbour.HexaPatternEven1 : Constants.Neighbour.HexaPatternOdd1;
        for (int i = 0; i < pattern.Length; i++)
        {
            neighbour = pattern[i] + cell;
            if (IsValidCell(neighbour.x, neighbour.y) &&
                !AgentNodeManager.IsHolding(neighbour.ZToZero()))
            {
                neighbours.Add(neighbour);
            }
        }
        return neighbours.ToArray();
    }

    public bool IsValidCell(int x, int y)
    {
        return Constants.IsValidCell(x, y);
    }
    #endregion

    public Vector3 CellToWorld(Vector3Int position)
    {
        return HexGrid.CellToWorld(position);
    }

    public Vector3Int WorldToCell(Vector3 position)
    {
        if (HexGrid != null)
            return HexGrid.WorldToCell(position);
        return Vector3Int.zero;
    }

    public List<Vector3> CellToWorld(List<Vector3Int> list)
    {
        List<Vector3> rs = new List<Vector3>();
        for (int i = 0; i < list.Count; i++)
        {
            rs.Add(CellToWorld(list[i]));
        }
        return rs;
    }

    public List<Vector3Int> WorldToCell(Vector3[] list)
    {
        List<Vector3Int> rs = new List<Vector3Int>();
        for (int i = 0; i < list.Length; i++)
        {
            rs.Add(WorldToCell(list[i]));
        }
        return rs;
    }

    public bool GetNearestPosition(Vector3Int center, out Vector3Int result)
    {
        return breathFirstSearch.GetNearestCell(center, out result);
    }
}
