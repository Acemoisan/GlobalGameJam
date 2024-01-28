using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WreckingBall : MonoBehaviour
{
    bool active = true;
    Vector3 defaultPos;

    void Start()
    {
        defaultPos = transform.position;
    }


    void Update()
    {
        if (active == false) return;
        transform.position += Vector3.right * Time.deltaTime * 10f;
    }

    public void ActivateWreckingBall()
    {
        active = true;
        StartCoroutine(ResetPosition());
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(5f);
        DefaultPosition();
    }

    public void DefaultPosition()
    {
        active = false;
        transform.position = defaultPos;
    }
}
