using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitDetection : MonoBehaviour
{
    [SerializeField] string interactableTag;

    [SerializeField] string objectiveTag;   
    [SerializeField] string finishLineTag; 
    Interactable interactee;
    public UnityEvent OnEnter;
    bool carryingObjective = false;
    GameObject objective;

    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter: " + other.name);
        if (other.CompareTag(interactableTag))
        {
            if (other.GetComponent<Interactable>() == null) { Debug.LogError("Interactable Component is null on, " + other); }

            interactee = other.GetComponent<Interactable>();

            Debug.Log("hit " + other.name);
            interactee.OnEnter();
            OnEnter?.Invoke();
        }

        else if (other.CompareTag(objectiveTag))
        {
            other.transform.parent = transform;
            carryingObjective = true;
            objective = other.gameObject;
        }

        else if (other.CompareTag(finishLineTag))
        {
            if (carryingObjective)
            {
                GameStateManager.instance.SavedPatient();
                DestroyObjective();
            }
        }
    }

    public void DestroyObjective()
    {
        Destroy(objective);
        carryingObjective = false;
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        // if (other.CompareTag(interactableTag))
        // {
        //     OnExit?.Invoke();
        // }
    }
}
