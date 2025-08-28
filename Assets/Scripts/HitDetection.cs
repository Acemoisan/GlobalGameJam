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

    
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag(interactableTag))
        {
            if (collision.gameObject.GetComponent<Interactable>() == null) { Debug.LogError("Interactable Component is null on, " + collision.gameObject); }

            interactee = collision.gameObject.GetComponent<Interactable>();

            //Debug.Log("hit " + collision.gameObject.name);
            AudioManager.instance.PlaySound(Sound.HitDetection);
            interactee.OnEnter();
            OnEnter?.Invoke();
        }

        // else if (collision.gameObject.CompareTag(objectiveTag))
        // {
        //     collision.gameObject.transform.parent = transform;
        //     carryingObjective = true;
        //     objective = collision.gameObject;
        // }

        else if (collision.gameObject.CompareTag(finishLineTag))
        {
            if (GameStateManager.instance != null)
            {
                GameStateManager.instance.SavedPatient();
                //DestroyObjective();
            }
        }
    }

    public void DestroyObjective()
    {
        Destroy(objective);
        carryingObjective = false;
    }

    void OnCollisionStay(Collision collision)
    {

    }

    void OnCollisionExit(Collision collision)
    {
        // if (collision.gameObject.CompareTag(interactableTag))
        // {
        //     OnExit?.Invoke();
        // }
    }
}
