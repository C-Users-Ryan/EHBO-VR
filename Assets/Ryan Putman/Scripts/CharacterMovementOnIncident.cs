using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CharacterMovementOnIncident : MonoBehaviour
{
    [SerializeField] private GameObject targetLocation;  // The target location set through the Inspector
    [SerializeField] private float delayBeforeMoving = 0f; // Delay in seconds before moving, default is 0
    [SerializeField] private GameObject objectToLookAt; // The object the character should look at upon arriving
    [SerializeField] private float rotationSpeed = 2f; // Speed of the rotation towards the object to look at

    private NavMeshAgent agent;
    private float originalSpeed; // Variable to store the original speed of the NavMeshAgent

    Animator m_Animator;

    private void Start()
    {
        // Ensure the character has a NavMeshAgent component
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("No NavMeshAgent found on this character!");
            return;
        }

        // Store the original speed of the NavMeshAgent
        originalSpeed = agent.speed;
        m_Animator = GetComponentInChildren<Animator>();
    }

    // This method will be called by the IncidentCountdown script when the incident is triggered
    public void OnIncidentTriggered()
    {
        if (targetLocation == null)
        {
            Debug.LogError($"{gameObject.name}: No target location set!");
            return;
        }

        if (agent == null)
        {
            Debug.LogError($"{gameObject.name}: NavMeshAgent not found!");
            return;
        }

        // Start the movement coroutine with a delay
        StartCoroutine(MoveToTargetAfterDelay());
    }

    private IEnumerator MoveToTargetAfterDelay()
    {
        // Wait for the specified delay before starting the movement
        yield return new WaitForSeconds(delayBeforeMoving);

        // Restore the agent's speed to its original value
        agent.speed = originalSpeed;

        // Check if the target is within the NavMesh area
        if (NavMesh.SamplePosition(targetLocation.transform.position, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            // Set the agent's destination to the target position
            agent.SetDestination(hit.position);
            agent.isStopped = false; // Ensure the agent is not stopped

            Debug.Log($"{gameObject.name} is moving to {hit.position}.");

            // Keep checking if the agent is still moving towards the target
            while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
            {
                // Optional: Add logic to react while moving (e.g., play walk animations, change state, etc.)
                yield return null;
            }

            // Call the OnArrivedAtLocation method once the destination is reached
            OnArrivedAtLocation();
        }
        else
        {
            Debug.LogError($"{gameObject.name}: Target location is not on the NavMesh!");
        }
    }

    // This function can be overridden or extended to add further actions once the character arrives at the target
    protected virtual void OnArrivedAtLocation()
    {
        // Placeholder for actions upon arrival, e.g., play an animation or change state
        Debug.Log($"{gameObject.name} has arrived at the target location.");
        agent.isStopped = true; // Stop the agent when the destination is reached

        m_Animator.SetBool("Arrived", true);

        // Start looking at the designated object, if specified
        if (objectToLookAt != null)
        {
            StartCoroutine(LookAtObject());
        }
    }

    private IEnumerator LookAtObject()
    {
        // Continuously rotate towards the object until the rotation is nearly complete
        while (true)
        {
            // Calculate the direction to the object to look at
            Vector3 directionToLook = objectToLookAt.transform.position - transform.position;
            directionToLook.y = 0; // Keep the character upright

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(directionToLook);

            // Smoothly rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

            // Check if the rotation is close enough to be considered complete
            if (Quaternion.Angle(transform.rotation, targetRotation) < 1f)
            {
                transform.rotation = targetRotation; // Snap to the target rotation
                yield break; // Stop the coroutine
            }

            yield return null;
        }
    }
}
