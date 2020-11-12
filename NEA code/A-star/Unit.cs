using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    Transform target;
    float speed = 20;
    Vector3[] path;
    int targetIndex;
    bool ismoving = false;

    FieldOfView fov;

    private void Awake()
    {
        fov = GetComponent<FieldOfView>();
    }
    void Update()
    {
        //fov.FindVisibleTargets();
        if(fov.visibleTargets.Count != 0 & !ismoving)
        {
            target = fov.visibleTargets[0];
            Debug.Log(target.transform.position);
            fov.visibleTargets.RemoveAt(0);
            PathRequestManager.RequestPath(transform.position, target.position, OnPathFound);
            ismoving = true;
        }

    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            for(int i = 0; i < path.Length; i++)
            {
                Debug.Log(path[i].ToString());
            }
            Debug.Log(path.Length);
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath", ismoving);
            
        }
    }

    IEnumerator FollowPath(bool ismoving)
    {
        Vector3 currentWaypoint = path[0] + Vector3.right;

        while (true)
        {
            if (transform.position == currentWaypoint)
            {
                targetIndex++;
                if (targetIndex >= path.Length)
                {
                    yield return new WaitForSeconds(3f);
                    HasArrived();
                    yield break;
                    
                }
                currentWaypoint = path[targetIndex] + Vector3.up ;
                Debug.Log(currentWaypoint.ToString());
                //currentWaypoint.y = 1;
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed * Time.deltaTime);
            yield return null;
        }
        
    }

    public void HasArrived()
    {
        Destroy(target.gameObject);
        ismoving = false;
    }
    public void OnDrawGizmos()
    {
        if (path != null)
        {
            for (int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if (i == targetIndex)
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
