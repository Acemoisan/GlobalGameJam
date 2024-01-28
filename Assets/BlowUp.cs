using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowUp : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    void Start()
    {
        Invoke("Explode", 2);
    }

    public void Explode()
    {
        Debug.Log("Explode");
        rb.AddExplosionForce(1000f, transform.position, 10f);
    }
}
