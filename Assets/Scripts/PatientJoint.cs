using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PatientJoint : MonoBehaviour
{
    [SerializeField] CharacterJoint joint;
    public UnityEvent TearLimb;


    public void TearOffLimb()
    {
        joint.breakForce = 0;
        TearLimb?.Invoke();
        transform.parent = null;
    }
}
