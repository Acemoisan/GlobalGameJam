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
    
    [Header("References")]
    [SerializeField] private Transform baseObject; // Reference to your base object
    private Transform currentInteractable; // This will be your debugTransform
    
    private Vector3 initialPosition;
    private Vector2 movementInput;

    void Update()
    {
        if (currentInteractable == null) return;
        
        // Apply movement based on input
        MoveInteractable();
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
        if (movementInput.magnitude < 0.1f) return; // Dead zone
        
        // Get the base's up vector as the reference direction
        Vector3 baseUp = baseObject.up;
        
        // Calculate movement using the base's up vector as reference
        Vector3 movement = new Vector3(movementInput.x, 0f, movementInput.y) * moveSpeed * Time.deltaTime;
        
        // Apply movement to the current interactable
        Vector3 newPosition = currentInteractable.position + movement;
        
        // Keep Y position the same
        newPosition.y = currentInteractable.position.y;
        currentInteractable.position = newPosition;
    }

    private Vector3 ClampPositionToRange(Vector3 position, Vector3 center, float range)
    {
        Vector3 offset = position - center;
        offset.y = 0f; // Keep Y at the same level
        
        if (offset.magnitude > range)
        {
            offset = offset.normalized * range;
        }
        
        return center + offset;
    }

    // Optional: Method to reset position to center
    public void ResetPosition()
    {
        if (currentInteractable != null)
        {
            currentInteractable.position = initialPosition;
        }
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
}
