using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndividualLevelController : MonoBehaviour
{
    [SerializeField] GameObject interactable;

    public GameObject GetInteractable()
    {
        return interactable;
    }
}
