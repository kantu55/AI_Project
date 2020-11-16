using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool displayGridGizmos;
    public Transform player;
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize; // グリッドのサイズ
    public float nodeRadius; // ノードの大きさ
    Node[,] grid; // グリッド

    float nodeDiameter; // ノードの直径
    int gridSizeX, gridSizeY; // グリッドの個数
    
    void Awake()
    {
        nodeDiameter = nodeRadius * 2;
        // 四捨五入して値を返す
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();
    }

    public int MaxSize
    {
        get
        {
            return gridSizeX * gridSizeY;
        }
    }

    // グリッドを作成
    void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        // 左下の座標
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                // 左下からグリッドを出していく
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                // DebugSphereを出して歩ける場所と歩けない場所を分けている
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeRadius, unwalkableMask));
                // グリッド作成
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }

    // 隣接ノードを取得する
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for(int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                    continue;

                int checkX = node.m_gridX + x;
                int checkY = node.m_gridY + y;

                // グリッドの範囲外であれば追加しない
                if(checkX >= 0 && checkX < gridSizeX &&
                    checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        }
        return neighbours;
    }

    // 現在の座標からどのグリッドに位置しているかを調べる
    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y;
        // 1に補間する
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        // 整数にする
        int x = Mathf.RoundToInt((gridSizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridSizeY - 1) * percentY);

        return grid[x, y];
    }
    
    // グリッドの表示
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
            if (grid != null && displayGridGizmos)
            {
                Node playerNode = GetNodeFromWorldPoint(player.position);
                foreach (Node n in grid)
                {
                    Gizmos.color = (n.m_Walkable) ? Color.white : Color.red;
                    Gizmos.DrawCube(n.m_WorldPosition, new Vector3((nodeDiameter - .1f), 0.5f, (nodeDiameter - .1f)));
                }
            }
    }
}
