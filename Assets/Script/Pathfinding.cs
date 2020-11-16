using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;

public class Pathfinding : MonoBehaviour
{
    PathRequestManager requestManager;
    Grid grid;

    void Awake()
    {
        requestManager = GetComponent<PathRequestManager>();
        grid = GetComponent<Grid>();
    }
    
    public void StartFindPath(Vector3 startPos, Vector3 targetPos)
    {
        StartCoroutine(FindPath(startPos, targetPos));
    }

    // スタートからゴールまでのパスを見つける
    IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();

        Vector3[] wayPoints = new Vector3[0];
        bool pathSuccess = false;

        Node startNode = grid.GetNodeFromWorldPoint(startPos);
        Node targetNode = grid.GetNodeFromWorldPoint(targetPos);

        if(startNode.m_Walkable && targetNode.m_Walkable)
        {
            Heap<Node> openSet = new Heap<Node>(grid.MaxSize);
            // HashSet...要素の重複を防ぐ
            HashSet<Node> closedSet = new HashSet<Node>();
            openSet.Add(startNode);

            // オープンリストが無くなるまで検索
            while (openSet.Count > 0)
            {
                Node currentNode = openSet.RemoveFirst();

                // クローズリストに追加
                closedSet.Add(currentNode);

                // 検索ノードがゴールに達したらパスを作る
                if (currentNode == targetNode)
                {
                    sw.Stop();
                    print("Path found: " + sw.ElapsedMilliseconds + " ms");
                    pathSuccess = true;
                    break;
                }

                foreach (Node neighbor in grid.GetNeighbours(currentNode))
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
        yield return null;
        if(pathSuccess)
        {
            wayPoints = RetracePath(startNode, targetNode);
        }
        requestManager.FinishedProcessingPath(wayPoints, pathSuccess);
    }

    // スタートからゴールまでのパスを作る
    Vector3[] RetracePath(Node startNode, Node endNode)
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

        // 進むポイントを決める
        Vector3[] wayPoints = SimplifyPath(path);

        // 反転してスタートからゴールまでのパスを作る
        Array.Reverse(wayPoints);
        return wayPoints;
    }

    // 進むポイントを減らす
    Vector3[] SimplifyPath(List<Node> path)
    {
        List<Vector3> wayPoints = new List<Vector3>();
        Vector2 directionOld = Vector2.zero;
        for(int i = 1; i < path.Count; i++)
        {
            // 次のノードの位置の差分を取る
            Vector2 directionNew = new Vector2(path[i - 1].m_gridX - path[i].m_gridX, path[i - 1].m_gridY - path[i].m_gridY);
            // 前回取った差分と不一致であれば、進むポイントとして登録する
            if(directionNew != directionOld)
            {
                wayPoints.Add(path[i].m_WorldPosition);
            }
            directionOld = directionNew;
        }
        return wayPoints.ToArray();
    }

    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.m_gridX - nodeB.m_gridX);
        int dstY = Mathf.Abs(nodeA.m_gridY - nodeB.m_gridY);

        // √2（三平方の定理）+直線の距離
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
