using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Acemoisan.Utils;
using UnityEngine.InputSystem;

public class AimController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float moveRange = 10f; // How far the object can move from center
    
    [Header("Inactivity Settings")]
    [SerializeField] private float inactivityTimeout = 60f; // Time in seconds before auto-quit triggers
    [SerializeField] private SceneLoader sceneLoader; // Reference to SceneLoader
    
    [Header("References")]
    [SerializeField] private Transform currentInteractable; // This will be your debugTransform
    
    private Vector3 initialPosition;
    private Vector2 movementInput;
    bool moveDelay = false;
    
    // Inactivity tracking
    private float lastMovementTime;
    private bool currentlyMoving = false;
    private bool autoQuitTriggered = false; // Flag to prevent multiple triggers

    void Start()
    {
        // Initialize inactivity tracking
        lastMovementTime = Time.time;
        currentlyMoving = false;
        autoQuitTriggered = false;
    }

    void Update()
    {
        if (currentInteractable == null) return;
        
        // Apply movement based on input
        MoveInteractable();
        
        // Check for inactivity
        CheckInactivity();
    }

    // This method will be called from your PlayerInput component's OnMovement event
    public void OnMovement(InputAction.CallbackContext context)
    {
        if (currentInteractable == null) return;
        
        // Get the 2D vector input from the PlayerInput system
        movementInput = context.ReadValue<Vector2>();
    }

    private void MoveInteractable()
    {
        if (movementInput.magnitude < 0.1f) { currentlyMoving = false; return; } // Dead zone
        if(moveDelay) { return; }
        
        // Reset inactivity timer when actually moving
        ResetInactivityTimer();
        
        // Calculate movement using the base's up vector as reference
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y) * moveSpeed * Time.deltaTime;
        
        // Apply movement to the current interactable
        Vector3 newPosition = currentInteractable.position + movement;
        
        // Keep Y position the same
        newPosition.y = currentInteractable.position.y;
        currentInteractable.position = newPosition;
    }

    private void CheckInactivity()
    {
        if (currentlyMoving) { 
            // Reset auto-quit flag when moving again
            if (autoQuitTriggered)
            {
                autoQuitTriggered = false;
                CancelAutoQuit();
            }
            return; 
        }
        
        if (Time.time - lastMovementTime >= inactivityTimeout && !autoQuitTriggered)
        {
            // Player has been inactive for the timeout period - trigger only once
            Debug.Log($"Player inactive for {inactivityTimeout} seconds. Starting auto-quit...");
            autoQuitTriggered = true;
            TriggerAutoQuit();
        }
    }

    private void ResetInactivityTimer()
    {
        lastMovementTime = Time.time;
        currentlyMoving = true;
    }

    private void TriggerAutoQuit()
    {
        if (sceneLoader != null)
        {
            sceneLoader.StartAutoQuitCoroutine();
        }
        else
        {
            Debug.LogError("SceneLoader reference not found in AimController!");
        }
    }

    private void CancelAutoQuit()
    {
        if (sceneLoader != null)
        {
            sceneLoader.StopAutoQuitCoroutine();
        }
        else
        {
            Debug.LogError("SceneLoader reference not found in AimController!");
        }
    }

    // Optional: Method to reset position to center
    public void ResetPosition()
    {
        if (currentInteractable != null)
        {
            currentInteractable.position = initialPosition;
        }

        ResetInactivityTimer();
    }

    public void SetInitialPosition(Vector3 position)
    {
        if (currentInteractable != null)
        {
            currentInteractable.position = position;
            initialPosition = position;
            moveDelay = true;
            Invoke(nameof(ResetMoveDelay), 1f); // Reset move delay after 0.5 seconds
        }

        ResetInactivityTimer();
    }

    private void ResetMoveDelay()
    {
        moveDelay = false;
    }

    // Optional: Method to change the current interactable at runtime
    public void SetCurrentInteractable(Transform newInteractable)
    {
        currentInteractable = newInteractable;
        if (currentInteractable != null)
        {
            initialPosition = currentInteractable.position;
        }
    }
    
    // Public method to manually reset inactivity timer (useful for other systems)
    public void ResetInactivityTimerPublic()
    {
        ResetInactivityTimer();
    }
    
    // Public method to get remaining inactivity time
    public float GetRemainingInactivityTime()
    {
        if (!currentlyMoving) return inactivityTimeout;
        return Mathf.Max(0, inactivityTimeout - (Time.time - lastMovementTime));
    }
}
