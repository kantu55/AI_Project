using System.Collections;
using UnityEngine;

public class Node
{
    public bool m_Walkable; // 歩行可能か
    public Vector3 m_WorldPosition; // ワールド座標

    public Node(bool walkble, Vector3 worldPosition)
    {
        m_Walkable = walkble;
        m_WorldPosition = worldPosition;
    }
}
