using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {

    [SerializeField]
	private Transform pathHolder;
    [SerializeField]
    private LayerMask obstacleMask;

    private Transform player;
    private Light spotlight;
    private Color origSpotlightColor;
    private float viewDist = 10f;
    private float viewAngle;
    private float speed = 5f;
    private float waitTime = 0.3f;
    private float turnSpeed = 45f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spotlight = GetComponentInChildren<Light>();
        viewAngle = spotlight.spotAngle;
        origSpotlightColor = spotlight.color;
    }

    private void Start()
    {
        Vector3[] waypoints = new Vector3[pathHolder.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = pathHolder.GetChild(i).transform.position;
            waypoints[i] = new Vector3(waypoints[i].x, transform.position.y, waypoints[i].z);
        }

        StartCoroutine(FollowPath(waypoints));
    }

    private void Update()
    {
        if (CanSeePlayer())
        {
            spotlight.color = Color.red;
        }
        else
        {
            spotlight.color = origSpotlightColor;
        }
    }

    private bool CanSeePlayer()
    {
        if (Vector3.Distance(transform.position, player.position) < viewDist)
        {
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float angleBetweenGuardAndPlayer = Vector3.Angle(transform.forward, dirToPlayer);

            if (angleBetweenGuardAndPlayer < viewAngle / 2f)
            {
                if (!Physics.Linecast(transform.position, player.position, obstacleMask))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private IEnumerator FollowPath (Vector3[] waypoints)
    {
        transform.position = waypoints[0];
        int targetWaypointIndex = 1;
        Vector3 targetWaypoint = waypoints[targetWaypointIndex];
        transform.LookAt(targetWaypoint);

        while (true)
        {
			transform.position = Vector3.MoveTowards (transform.position, targetWaypoint, speed * Time.deltaTime);
			if (transform.position == targetWaypoint) {
				targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;
				targetWaypoint = waypoints [targetWaypointIndex];
				yield return new WaitForSeconds (waitTime);
				yield return StartCoroutine (TurnToFace (targetWaypoint));
			}
			yield return null;
        }
    }

    private IEnumerator TurnToFace (Vector3 lookTarget)
    {
        Vector3 dirToLookTarget = (lookTarget - transform.position).normalized;
        float targetAngle = 90 - Mathf.Atan2(dirToLookTarget.z, dirToLookTarget.x) * Mathf.Rad2Deg;

        while (Mathf.Abs(Mathf.DeltaAngle(transform.eulerAngles.y, targetAngle)) > 0.01f)
        {
            float angle = Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetAngle, turnSpeed * Time.deltaTime);
            transform.eulerAngles = Vector3.up * angle;
            yield return null;
        }
    }

    private void OnDrawGizmos ()
    {
        Vector3 startPos = pathHolder.GetChild(0).transform.position;
        Vector3 previousPos = startPos;
        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPos, waypoint.position);
            previousPos = waypoint.position;
        }
        Gizmos.DrawLine(previousPos, startPos);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * viewDist);
    }

}
