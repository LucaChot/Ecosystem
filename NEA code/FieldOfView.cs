using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;

	LayerMask targetMask;
	public LayerMask obstacleMask;
	public Transform closestTarget;
	float closestDst;

	[HideInInspector]
	public List<Transform> visibleTargets = new List<Transform>();
	
    /*void Start()
	{
		StartCoroutine("FindTargetsWithDelay", .2f);
	}


	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}*/

	public Transform  FindVisibleTargets(LayerMask mask)
	{
		targetMask = mask;
		closestDst = 999;
		closestTarget = null;
		visibleTargets.Clear();
		Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

		for (int i = 0; i < targetsInViewRadius.Length; i++)
		{
			Transform target = targetsInViewRadius[i].transform;
			//Debug.Log("Found");
			if (transform.gameObject != target.gameObject)
            {
				
				Vector3 dirToTarget = (target.position - transform.position).normalized;
				if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
				{
					
					float dstToTarget = Vector3.Distance(transform.position, target.position);
					
					//if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget))
					//{
						
					visibleTargets.Add(target);
					if (dstToTarget < closestDst)
					{
						closestTarget = target;
						closestDst = dstToTarget;
							
					}
					
					//}
				}
			}			
		}
		return closestTarget;
	}


	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += transform.eulerAngles.y;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
	}
}