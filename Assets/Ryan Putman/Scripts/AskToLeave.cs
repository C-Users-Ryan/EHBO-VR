using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AskToLeave : MonoBehaviour
{
    [SerializeField] private GameObject targetLocation;  // The target location set through the Inspector
    [SerializeField] private float delayBeforeMoving = 0f; // Delay in seconds before moving, default is 0
    [SerializeField] private GameObject objectToLookAt; // The object the character should look at upon arriving
    [SerializeField] private float rotationSpeed = 2f; // Speed of the rotation towards the object to look at

    private NavMeshAgent agent;
    private float originalSpeed; // Variable to store the original speed of the NavMeshAgent
    private bool moveTriggered = false; // Tracks if the "M" key has been pressed

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

        // Store the original speed of the agent
        originalSpeed = agent.speed;

        // Get Animator component
        m_Animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Check if the "M" key has been pressed
        if (Input.GetKeyDown(KeyCode.M))
        {
            moveTriggered = true;
            StartCoroutine(MoveToTarget());
        }
    }

    private IEnumerator MoveToTarget()
    {
        if (moveTriggered)
        {
            // Delay before movement
            yield return new WaitForSeconds(delayBeforeMoving);

            // Controleer of de NavMeshAgent actief is
            if (!agent.enabled)
            {
                Debug.LogWarning("NavMeshAgent is disabled, enabling it now.");
                agent.enabled = true;  // Zet de NavMeshAgent weer aan als deze is uitgeschakeld
            }

            // Restore the agent's speed to its original value
            agent.speed = originalSpeed;
            agent.SetDestination(targetLocation.transform.position);

            // Update animation (optional, depending on your setup)
            if (m_Animator != null)
            {
                m_Animator.SetBool("ask", true);
            }


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
            }

                // Stop walking animation after reaching destination
                if (m_Animator != null)
            {
                m_Animator.SetBool("ask", false);
            }

            // Start looking at the object after arrival
            if (objectToLookAt != null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(objectToLookAt.transform.position - transform.position);
                while (Quaternion.Angle(transform.rotation, targetRotation) > 0.1f)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                    yield return null;
                }
            }
        }
    }
}
