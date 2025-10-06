using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptbasisdetectie : MonoBehaviour
{
    [SerializeField] private float requiredDuration = 2.0f;
    [SerializeField] private clipboard clipboardTasks;
    [SerializeField] private string taskToComplete;

    [Header("Trigger Settings")]
    [SerializeField] private List<GameObject> objectsToDeActivateOnEnter;
    [SerializeField] private List<GameObject> objectsToActivateOnExit;

    private float actionTimer = 0.0f;
    private bool isPerformingAction = false;
    private BoxCollider boxCollider;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPerformingAction = true;

            if (boxCollider != null)
                boxCollider.size *= 3f;

            foreach (GameObject obj in objectsToDeActivateOnEnter)
                obj.SetActive(false);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPerformingAction = false;
            actionTimer = 0.0f;

            if (boxCollider != null)
                boxCollider.size /= 3f;

            foreach (GameObject obj in objectsToActivateOnExit)
                obj.SetActive(true);
        }
    }

    void Update()
    {
        if (isPerformingAction)
        {
            actionTimer += Time.deltaTime;

            if (actionTimer >= requiredDuration)
            {
                CompleteTask();
                isPerformingAction = false;
            }
        }
    }

    public void CompleteTask()
    {
        if (clipboardTasks != null)
        {
            clipboardTasks.RegisterTaskCompletion(taskToComplete);
        }
        else
        {
            Debug.LogError("ClipboardTasks reference is not assigned in the inspector.");
        }
    }
}