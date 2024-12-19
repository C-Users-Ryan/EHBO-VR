using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    [SerializeField] private Transform target; // The target game object to face

    private void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("Target not assigned for " + gameObject.name);
            return;
        }

        // Calculate the direction to the target
        Vector3 directionToTarget = target.position - transform.position;

        // Calculate the angle for the X-axis rotation
        float angle = -Mathf.Atan2(directionToTarget.z, directionToTarget.y) * Mathf.Rad2Deg;

        // Maintain the original Y and Z rotations while updating the X-axis
        transform.rotation = Quaternion.Euler(angle, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
    }
}
