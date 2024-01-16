using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {
    /* MEMBER VARIABLES *///==================================================
    EnemyWaypointManager waypointManager;

    const float MoveSpeed = 1; // moves 'MoveSpeed' distance per a second

    List<Transform> waypoints;



    /* UNITY EVENT FUNCTIONS *///==================================================
    void Awake() {
        GetWaypoints();
    }



    /* PUBLIC METHODS *///==================================================
    public void InitializeMovement() {
        JumpToFirstWaypoint();
        StartCoroutine(FollowWaypoints());
    }



    /* PRIVATE METHODS *///==================================================
    void GetWaypoints() {
        waypoints = new List<Transform>();
        waypointManager = FindObjectOfType<EnemyWaypointManager>();

        foreach (Transform t in waypointManager.transform) {
            waypoints.Add(t);
        }
    }
    void JumpToFirstWaypoint() {
        transform.position = waypoints[0].position;
    }



    /* IENUMERATORS *///==================================================
    IEnumerator FollowWaypoints() {
        yield return new WaitForEndOfFrame();

        foreach (Transform waypoint in waypoints) {
            Vector3 startPosition = transform.position;
            Vector3 targetPosition = waypoint.position;
            float targetDistance = Vector3.Distance(startPosition, targetPosition);
            float progressRate = 0f;

            while (progressRate < 1f) {
                progressRate = progressRate + Time.deltaTime * (MoveSpeed / targetDistance);
                if (progressRate > 1f)
                    progressRate = 1f;

                transform.position = Vector3.Lerp(startPosition, targetPosition, progressRate);

                yield return new WaitForEndOfFrame();
            }
        }

        gameObject.SetActive(false);
        yield return null;
    }
}
