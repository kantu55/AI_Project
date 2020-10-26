using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepController : MonoBehaviour
{
    GameObject target; // 目標とするオブジェクト
    Astar astar; // 経路探索用のファイル
    List<Vector3> points; // 経路探索で出たポイントを格納するリスト
    int currentPoint; // 現在向かっている経路ポイント
    int pointLength; // 目的地までの経路探索ポイント数
    public int wayLength; // 現在の探索ポイントから次の探索ポイントの距離
    Vector3 direction;
    Quaternion rotation; // 回転
    Rigidbody rb;
    private int jumpIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.Find("Target");
        astar = GetComponent<Astar>();
        // 目標地点までの経路探索を行う
        points = astar.Search(this.gameObject, target, wayLength);
        jumpIndex = astar.jumpIndex;
        pointLength = points.Count;
        currentPoint = 1;
    }

    void Update()
    {
        direction = points[currentPoint] - this.gameObject.transform.position;
        if(currentPoint == jumpIndex)
        {
            transform.position = points[currentPoint];
        }
        else
        {
            // 方向転換
            rotation = Quaternion.LookRotation(direction);
            transform.rotation = new Quaternion(transform.rotation.x, Quaternion.Slerp(transform.rotation, rotation, 2 * Time.deltaTime).y,
                transform.rotation.z, transform.rotation.w);
            // 前方ベクトルに進む
            transform.Translate(0, 0, Time.deltaTime * 2);
        }

        // 経路ポイントに近づいたら次の経路ポイントに進む
        if (direction.magnitude <= 1)
        {
            ++currentPoint;
            // 目標地点まで進んだら新しく目的地まで経路探索を行う
            if (currentPoint == pointLength)
            {
                currentPoint = 1;
                astar = GetComponent<Astar>();
                points = astar.Search(this.gameObject, target, wayLength);
                pointLength = points.Count;
            }
        }
    }

    // ただのデバッグ
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        // なんかエラー出る。。。
        if (currentPoint > 1)
        {
            for(int i = 1; i < pointLength; i++)
            {
                Gizmos.DrawSphere(points[i], 0.5f);
            }
        }
    }
}
