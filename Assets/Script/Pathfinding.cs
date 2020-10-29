using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public Transform seeker, target;
    Grid grid;

    void Awake()
    {
        grid = GetComponent<Grid>();
    }

    void Update()
    {
        FindPath(seeker.position, target.position);
    }

    // スタートからゴールまでのパスを見つける
    void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        // HashSet...要素の重複を防ぐ
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        // オープンリストが無くなるまで検索
        while(openSet.Count > 0)
        {
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++)
            {
                /*
                * 現在辿っているノードがオープンノードのトータルコストより高い
                * 又は
                * 現在辿っているノードとオープンノードのトータルコストが等しい 且つ
                * 現在辿っているノードがオープンノードのヒューリスティックコストより高い
                */
                if (openSet[i].fCost < currentNode.fCost ||
                    openSet[i].fCost == currentNode.fCost && openSet[i].hCost < currentNode.hCost)
                {
                    currentNode = openSet[i];
                }
            }

            // オープンリストから削除
            openSet.Remove(currentNode);
            // クローズリストに追加
            closedSet.Add(currentNode);

            // 検索ノードがゴールに達したらパスを作る
            if(currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach(Node neighbor in grid.GetNeighbours(currentNode))
            {
                // 歩けない場所であるか隣接ノードにクローズリストがあれば次へ
                if (!neighbor.m_Walkable || closedSet.Contains(neighbor))
                    continue;

                // 現在のノードから隣接ノードまでの距離コストを出す
                int MovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbor);

                /* 現在のノードから隣接ノードまでの距離コストより隣接頂点間ノードが高い
                 * 又は
                 * 隣接ノードがオープンノードになければ隣接ノードとして登録
                */
                if (MovementCostToNeighbour < neighbor.gCost || !openSet.Contains(neighbor))
                {
                    neighbor.gCost = MovementCostToNeighbour;
                    neighbor.hCost = GetDistance(neighbor, targetNode);
                    neighbor.parent = currentNode;

                    // Openノードに隣接ノードが入っていなければ追加
                    // Contains...要素の検索
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
    }

    // スタートからゴールまでのパスを作る
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        // ゴールのノードからスタート
        Node currentNode = endNode;

        // ゴールからスタートまでノードを辿る
        while(currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }

        // 反転してスタートからゴールまでのパスを作る
        path.Reverse();
        
        //グリッドに反映
        grid.path = path;
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.m_gridX - nodeB.m_gridX);
        int dstY = Mathf.Abs(nodeA.m_gridY - nodeB.m_gridY);

        // 14y + 10(x-y)で距離求まるらしい（＃＾ω＾）・・・
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
