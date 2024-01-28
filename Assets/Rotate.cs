using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] bool active = false;
    Vector3 defaultRot;

    void Start()
    {
        defaultRot = transform.rotation.eulerAngles;
    }

    void Update()
    {
        if(active == false) return;
        transform.Rotate(-Vector3.forward * 100 * Time.deltaTime);
    }


    public void ActivateRotation()
    {
        active = true;
        StartCoroutine(ResetRotation());
    }

    IEnumerator ResetRotation()
    {
        yield return new WaitForSeconds(5f);
        transform.rotation = new Quaternion(0, 0, 0, 0);
    }
}
