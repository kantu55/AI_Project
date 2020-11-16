using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PathRequestManager : MonoBehaviour
{
    Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
    PathRequest currentPathRequest; // 現在のパスリクエスト

    static PathRequestManager instance;
    Pathfinding pathfinding;

    bool isProcessPath; // パスを辿っているか

    void Awake()
    {
        instance = this;
        pathfinding = GetComponent<Pathfinding>();
    }

    public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, Action<Vector3[], bool> callback)
    {
        PathRequest newRequest = new PathRequest(pathStart, pathEnd, callback);
        // インスタンスに登録
        instance.pathRequestQueue.Enqueue(newRequest);
        instance.TryProcessNext();
    }

    // 次のパス検索を処理
    void TryProcessNext()
    {
        if(!isProcessPath && pathRequestQueue.Count > 0)
        {
            // 格納されているパスを取り出す
            currentPathRequest = pathRequestQueue.Dequeue();
            isProcessPath = true;
            pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
        }
    }

    // パス検索終了処理
    public void FinishedProcessingPath(Vector3[] path, bool success)
    {
        currentPathRequest.callback(path, success);
        isProcessPath = false;
        TryProcessNext();
    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            callback = _callback;
        }
    }
}
