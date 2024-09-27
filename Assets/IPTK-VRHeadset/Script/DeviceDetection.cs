using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class DeviceDetection : MonoBehaviour
{
    // Placeholder for other manager scripts (e.g., PlayerController)
    public GameObject vrControllerPrefab;
    public GameObject desktopControllerPrefab;

    private GameObject currentController;

    void Start()
    {
        DetectInputDevice();
    }

    void DetectInputDevice()
    {
        if (XRSettings.isDeviceActive)
        {
            Debug.Log("VR Headset is active");
            InitializeVRControls();
        }
        else
        {
            Debug.Log("Using Laptop/Desktop");
            InitializeDesktopControls();
        }
    }

    void InitializeVRControls()
    {
        if (vrControllerPrefab != null)
        {
            currentController = Instantiate(vrControllerPrefab);
            // 在这里可以初始化手势控制，VR控制器输入等
            Debug.Log("VR Controls Initialized");
        }
        else
        {
            Debug.LogWarning("VR Controller Prefab is not assigned.");
        }
    }

    void InitializeDesktopControls()
    {
        if (desktopControllerPrefab != null)
        {
            currentController = Instantiate(desktopControllerPrefab);
            // 初始化鼠标和键盘控制
            Debug.Log("Desktop Controls Initialized");
        }
        else
        {
            Debug.LogWarning("Desktop Controller Prefab is not assigned.");
        }
    }
}
