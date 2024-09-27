using UnityEngine;
using UnityEngine.XR;

public class AppInitialisation : MonoBehaviour
{
    public GameObject desktopCamera;  // Desktop camera GameObject
    public GameObject vrHeadsetCamera;  // VR camera GameObject
    public Canvas mainCanvas;  // Reference to the Canvas

    void Start()
    {
        // Step 1: Detect the input device (laptop or VR headset)
        print("Start app setup");
        DetectInputDevice();
    }

    // This function detects which input device is being used
    void DetectInputDevice()
    {
        if (XRSettings.isDeviceActive)
        {
            Debug.Log("VR Headset detected. Initializing VR controls...");
            // Step 3: Activate VR Camera and deactivate the desktop camera
            InitializeVRSetup();
        }
        else
        {
            Debug.Log("Laptop/Desktop detected. Initializing desktop controls...");
            // Step 3: Activate desktop Camera and deactivate the VR camera
            InitializeDesktopSetup();
        }
    }

    // Step 3: Initialize desktop setup
    void InitializeDesktopSetup()
    {
        desktopCamera.SetActive(true);  // Activate desktop camera
        vrHeadsetCamera.SetActive(false);  // Deactivate VR camera

        // Step 4: Set the Canvas Event Camera to the desktop camera
        SetCanvasCamera(desktopCamera.GetComponent<Camera>());

        // Display the Canvas in front of the user
        PositionCanvas(desktopCamera.transform);
    }

    // Step 3: Initialize VR setup
    void InitializeVRSetup()
    {
        desktopCamera.SetActive(false);  // Deactivate desktop camera
        vrHeadsetCamera.SetActive(true);  // Activate VR camera

        // Step 4: Set the Canvas Event Camera to the VR camera
        SetCanvasCamera(vrHeadsetCamera.GetComponent<Camera>());

        // Display the Canvas in front of the user
        PositionCanvas(vrHeadsetCamera.transform);
    }

    // Step 4: Assign the correct camera to the Canvas event camera
    void SetCanvasCamera(Camera cam)
    {
        if (mainCanvas != null)
        {
            mainCanvas.worldCamera = cam;
            Debug.Log("Canvas Event Camera set to: " + cam.name);
        }
        else
        {
            Debug.LogError("Canvas reference is missing!");
        }
    }

    // Step 2: Position the Canvas in front of the user
    void PositionCanvas(Transform cameraTransform)
    {
        if (mainCanvas != null)
        {
            // Set Canvas position in front of the user
            mainCanvas.transform.position = cameraTransform.position + cameraTransform.forward * 2f;  // Position 2 units in front of the camera
            mainCanvas.transform.rotation = cameraTransform.rotation;  // Match camera rotation
            mainCanvas.gameObject.SetActive(true);  // Ensure the canvas is active
        }
        else
        {
            Debug.LogError("Canvas reference is missing!");
        }
    }
}
