using System.Collections;
using UnityEngine;

public class Node
{
    public bool m_Walkable; // 歩行可能か
    public Vector3 m_WorldPosition; // ワールド座標
    public int m_gridX;
    public int m_gridY;

    public int gCost;
    public int hCost;
    public Node parent;

    public Node(bool walkble, Vector3 worldPosition, int gridX, int gridY)
    {
        m_Walkable = walkble;
        m_WorldPosition = worldPosition;
        m_gridX = gridX;
        m_gridY = gridY;
    }

    // 最短コストを取得
    public int fCost
    {
        get
        {
            return gCost + hCost;
        }
    }
}
