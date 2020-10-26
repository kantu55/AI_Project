using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : MonoBehaviour
{
    GameObject target; // 目標とするオブジェクト
    Astar astar; // 経路探索用のファイル
    List<Vector3> points; // 経路探索で出たポイントを格納するリスト
    int currentPoint; // 現在向かっている経路ポイント
    int pointLength; // 目的地までの経路探索ポイント数
    public int wayLength; // 現在の探索ポイントから次の探索ポイントの距離
    Vector3 direction;

    void Start()
    {
        target = GameObject.Find("Target");
        astar.GetComponent<Astar>();
        points = astar.Search(this.gameObject, target, wayLength);
        pointLength = points.Count;
        currentPoint = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetMovePos(int index)
    {
        if(currentPoint < points.Count)
        {
            currentPoint = index;
            return points[currentPoint];
        }
        return points[currentPoint];
    }
}
