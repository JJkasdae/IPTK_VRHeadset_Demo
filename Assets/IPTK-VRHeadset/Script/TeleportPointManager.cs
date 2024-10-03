using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus; // Ensure the Oculus SDK is imported
using UnityEngine.InputSystem;

public class TeleportPointManager : MonoBehaviour
{
    private List<TeleportPoint> teleportPoints = new List<TeleportPoint>();

    // Define the controller's ray length
    private float vrRayLength = 10.0f;

    private InputAction mouseRightClickAction;

    void Start()
    {
        // Initialize and find all teleport points in the scene
        teleportPoints = new List<TeleportPoint>(FindObjectsOfType<TeleportPoint>());

        mouseRightClickAction = new InputAction(binding: "<Mouse>/rightButton");
        mouseRightClickAction.Enable();
    }

    private void OnDisable()
    {
        if (mouseRightClickAction != null)
        {
            mouseRightClickAction.Disable();
        }
    }

    public Vector3? CheckForTeleportPointClick(Camera playerCamera)
    {
        // Check for mouse input (Desktop mode)
        if (mouseRightClickAction.triggered) // Right-click
        {
            Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            return CheckRaycastHit(ray);
        }

        return null;
    }

    private Vector3? CheckRaycastHit(Ray ray)
    {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            TeleportPoint teleportPoint = hit.collider.GetComponent<TeleportPoint>();
            if (teleportPoint != null)
            {
                return teleportPoint.GetPosition();
            }
        }

        return null;
    }
}
