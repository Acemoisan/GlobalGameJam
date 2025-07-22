using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalInput : MonoBehaviour
{
    [SerializeField] SceneLoader sceneLoader;
    //[SerializeField] public UnityEvent onInteract;
    private bool interacted = false;

    // Update is called once per frame
    void Update()
    {
        //if mouse 3 auto quit 
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (interacted) { return; }
            interacted = true;

            Debug.Log("Quitting game...");
            sceneLoader.QuitGame();
        }

        // Check for touch input anywhere on screen
        // if (Input.touchCount > 0)
        // {
        //     Touch touch = Input.GetTouch(0);
            
        //     if (touch.phase == TouchPhase.Began)
        //     {
        //         Debug.Log("Screen touched - invoking onInteract event");
        //         onInteract?.Invoke();
        //     }
        // }

        // // Also check for mouse click for testing in editor
        // if (Input.GetMouseButtonDown(0))
        // {
        //     Debug.Log("Mouse clicked - invoking onInteract event");
        //     onInteract?.Invoke();
        // }
    }
}
