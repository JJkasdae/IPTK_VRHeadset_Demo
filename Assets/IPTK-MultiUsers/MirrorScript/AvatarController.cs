using Mirror;
using Oculus.Platform;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour 
{
    public Transform headTransform; 
    public Transform vrCameraTransform; 
    public Transform desktopCameraTransform;
    private GameObject vrCamera;
    private GameObject desktopCamera;

    private bool isVRCameraActive;

    void Start()
    {

        vrCamera = transform.parent.Find("VRHeadsetCamera").gameObject;
        desktopCamera = transform.parent.Find("DesktopCamera").gameObject;
        
    }

    void Update()
    {

        if (vrCamera.activeSelf)
        {
            headTransform.position = vrCameraTransform.position;
            headTransform.rotation = vrCameraTransform.rotation;
        }
        else
        {
            headTransform.position = desktopCameraTransform.position;
            headTransform.rotation = desktopCameraTransform.rotation;
        }
    }
}
