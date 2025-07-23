using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] List<Transform> waypoints = new List<Transform>();
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float waitTime = 1f;
    
    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    
    void Start()
    {
        if (waypoints.Count == 0)
        {
            Debug.LogWarning("No waypoints assigned to Patrol script on " + gameObject.name);
            return;
        }
        
        // Start at the first waypoint
        transform.position = waypoints[0].position;
    }
    
    void Update()
    {
        if (waypoints.Count == 0 || isWaiting) return;
        
        // Move towards current waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;
        
        // Check if we've reached the waypoint
        float distanceToWaypoint = Vector3.Distance(transform.position, targetWaypoint.position);
        if (distanceToWaypoint < 0.01f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }
    
    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        
        // Move to next waypoint
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
        isWaiting = false;
    }
    
    // Optional: Draw gizmos in the scene view to visualize waypoints
    void OnDrawGizmosSelected()
    {
        if (waypoints.Count == 0) return;
        
        Gizmos.color = Color.yellow;
        
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i] == null) continue;
            
            // Draw waypoint
            Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);
            
            // Draw line to next waypoint
            int nextIndex = (i + 1) % waypoints.Count;
            if (waypoints[nextIndex] != null)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
            }
        }
    }
} 