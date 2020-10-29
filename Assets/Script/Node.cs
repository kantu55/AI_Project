using System.Collections;
using UnityEngine;

public class Node
{
    public bool m_Walkable; // 歩行可能か
    public Vector3 m_WorldPosition; // ワールド座標
    public int m_gridX; // グリッドNo（X座標）
    public int m_gridY; // グリッドNo（Y座標）
    public int gCost; // ノード間のコスト
    public int hCost; // ヒューリスティックコスト
    public Node parent; // 親ノード

    public Node(bool walkble, Vector3 worldPosition, int gridX, int gridY)
    {
        m_Walkable = walkble;
        m_WorldPosition = worldPosition;
        m_gridX = gridX;
        m_gridY = gridY;
    }

    // トータル（最短）コストを取得
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
