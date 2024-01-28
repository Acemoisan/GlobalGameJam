using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acemoisan.Utils;
using UnityEngine.InputSystem;


public class AimController : MonoBehaviour
{
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject debugTransform;
    [SerializeField] LayerMask aimCollider;
    [SerializeField] LayerMask bodyCollider;
    [SerializeField] LineRenderer aimLineRenderer;

    //public Vector3 AimWorldPosition { get { return aimWorldPosition; } }
    Vector3 aimWorldPosition;
    Vector3 aimInput;
    bool canAim = false;



    void Start()
    {
        debugTransform.SetActive(true);
        aimInput = (aimInput * 0.5f + Vector3.one * 0.5f) * new Vector2(Screen.width, Screen.height);
        GetAimWorldPosition(_mainCamera, aimInput);
        debugTransform.transform.position = aimWorldPosition;        
        Invoke("CanAim", 3f);
    }

    void CanAim()
    {
        canAim = true;
    }

void Update()
{
    if(canAim == false) return;
    if (Gamepad.current != null)
    {
        aimInput = Gamepad.current.rightStick.ReadValue();

        //Debug.Log("Controller is working and on " + aimInput);
        aimInput = (aimInput * 0.5f + Vector3.one * 0.5f) * new Vector2(Screen.width, Screen.height);
    }
    else
    {
        aimInput = Input.mousePosition;
    }


    GetAimWorldPosition(_mainCamera, aimInput);
    debugTransform.transform.position = aimWorldPosition;
}


    void GetAimWorldPosition(Camera camera, Vector3 aimPos)
    {
        if (camera != null)
        {
            Ray ray = camera.ScreenPointToRay(aimPos);
            if (Physics.Raycast(ray, out RaycastHit bodyHit, 1000f, bodyCollider))
            {
                aimWorldPosition = bodyHit.point;
            }
            else if (Physics.Raycast(ray, out RaycastHit hit, 1000f, aimCollider))
            {
                aimWorldPosition = hit.point;
            }
            else
            {
                aimWorldPosition = Vector3.zero;
            }
        }
    }



}
