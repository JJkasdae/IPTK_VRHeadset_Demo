using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Oculus; // Ensure the Oculus SDK is imported

public class TeleportPointManager : MonoBehaviour
{
    private List<TeleportPoint> teleportPoints = new List<TeleportPoint>();

    // Define the controller's ray length
    private float vrRayLength = 10.0f;

    void Start()
    {
        // Initialize and find all teleport points in the scene
        teleportPoints = new List<TeleportPoint>(FindObjectsOfType<TeleportPoint>());
    }

    public Vector3? CheckForTeleportPointClick(Camera playerCamera)
    {
        // Check for mouse input (Desktop mode)
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            return CheckRaycastHit(ray);
        }

        // Check for VR controller input
        //if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger)) // Trigger button press
        //{
        //    Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward); // Ray from the VR headset's forward direction
        //    return CheckRaycastHit(ray);
        //}

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
