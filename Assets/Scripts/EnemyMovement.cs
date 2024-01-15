using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    /* MEMBER VARIABLES */
    EnemyWaypointManager waypointManager;
    List<Transform> waypoints;

    float moveSpeed = 1; // moves 'moveSpeed' distance per a second



    /* UNITY EVENT FUNCTIONS */
    void Awake() {
        GetWaypointsFromManager();
    }
    void OnEnable() {
        JumpToFirstWaypoint();
        StartCoroutine(FollowWaypoints());
    }



    /* PRIVATE METHODS */
    void GetWaypointsFromManager() {
        waypoints = new List<Transform>();
        waypointManager = FindObjectOfType<EnemyWaypointManager>();

        foreach (Transform t in waypointManager.transform) {
            waypoints.Add(t);
        }
    }
    void JumpToFirstWaypoint()
    {
        transform.position = waypoints[0].position;
    }



    /* IENUMERATORS */
    IEnumerator FollowWaypoints()
    {
        yield return new WaitForEndOfFrame();

        foreach (Transform waypoint in waypoints) {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = waypoint.position;
            float targetDistance = Vector3.Distance(startPosition, targetPosition);
            float progressRate = 0f;

            while (progressRate < 1f) {
                progressRate = progressRate + Time.deltaTime * (moveSpeed / targetDistance);
                if (progressRate > 1f)
                    progressRate = 1f;

                transform.position = Vector3.Lerp(startPosition, targetPosition, progressRate);

                yield return new WaitForEndOfFrame();
            }
        }

        yield return null;
    }
}
