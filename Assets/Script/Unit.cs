using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Transform target;
    Vector3 currentTargetPoint;
    public float speed = 1;
    Vector3[] path;
    int targetIndex;
    bool pathSuccess = false;
    Vector3 currentWayPoint;

    void Start()
    {
        currentTargetPoint = target.position;
        PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
    }

    private void Update()
    {
        if(pathSuccess)
        {
            if (transform.position == currentWayPoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length || target.position != currentTargetPoint)
                {
                    pathSuccess = false;
                }
                else
                {
                    currentWayPoint = path[targetIndex];
                }
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
        }
        else
        {
            if(target.position != currentTargetPoint)
            {
                PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
                currentTargetPoint = target.position;
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if(pathSuccessful && newPath.Length > 0)
        {
            targetIndex = 0;
            path = newPath;
            currentWayPoint = path[0];
            pathSuccess = true;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);
                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
