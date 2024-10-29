using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public interface WalkInteractable
{
    void Interact();
}
public class MoveToWaypoint : MonoBehaviour
{
    private NavMeshAgent agent;
    public GameObject waypointPath; // Reference to the parent object holding waypoints
    private Transform[] waypoints;

    public float minDistance = 8f;
    public float speed = 5.0f;

    public Transform assignedWaypoint; // Assigned waypoint to switch to after stopping

    private int currentWaypointIndex;
    private bool isPerformingAction = false;
    private bool hasStopped = false; // New flag to check if the agent has stopped

    Animator m_Animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        m_Animator = GetComponentInChildren<Animator>();
        m_Animator.SetBool("Walking", true);
        waypoints = new Transform[waypointPath.transform.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointPath.transform.GetChild(i);
        }

        ChooseNewRandomWaypoint();
    }

    void Update()
    {
        // Check if the speed has been set to 0, either by the IncidentCountdown or other scripts
        if (agent.speed == 0 && !hasStopped)
        {
            // Immediately stop and switch to assigned waypoint
            StopAndSwitchToAssignedWaypoint();
        }
        else if (!isPerformingAction && !hasStopped)
        {
            MoveTowardsWaypoint();
        }
    }

    void ChooseNewRandomWaypoint()
    {
        currentWaypointIndex = Random.Range(0, waypoints.Length);
    }

    public void MoveTowardsWaypoint()
    {
        // Calculate the distance to the current waypoint
        float distance = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

        // If within the minimum distance, interact with the WalkInteractable component
        if (distance < minDistance)
        {
            WalkInteractable interactable = waypoints[currentWaypointIndex].GetComponent<WalkInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
                isPerformingAction = true; // Prevent further movement until action is complete
                m_Animator.SetBool("Walking", false);
            }
        }
        else
        {
            // Move towards the current waypoint
            agent.SetDestination(waypoints[currentWaypointIndex].position);
            agent.speed = speed;
        }
    }

    // Immediately stop and switch to the assigned waypoint
    void StopAndSwitchToAssignedWaypoint()
    {
        hasStopped = true; // Mark that the agent has stopped

        // Stop all movement
        agent.isStopped = true;
        m_Animator.SetBool("Walking", false);

        // Set the destination to the assigned waypoint, even though it won't move
        if (assignedWaypoint != null)
        {
            // Set the destination (to ensure agent knows where the waypoint is)
            agent.SetDestination(assignedWaypoint.position);

            // Make the character face the assigned waypoint
            LookAtWaypoint(assignedWaypoint);
        }
    }

    // Function to smoothly look at the assigned waypoint
    void LookAtWaypoint(Transform waypoint)
    {
        Vector3 direction = (waypoint.position - transform.position).normalized;
        direction.y = 0; // Prevent tilting the character up/down
        if (direction != Vector3.zero)
        {
            // Instantly rotate to face the target waypoint
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 1f); // Set rotation instantly
        }
    }

    public void nextWaypoint()
    {
        if (!hasStopped)
        {
            ChooseNewRandomWaypoint();
            isPerformingAction = false;
            m_Animator.SetBool("Walking", true);
        }
    }

    // Function to disable this script
/*        public void DisableScript()
        {
            this.enabled = false;
        }*/
}
