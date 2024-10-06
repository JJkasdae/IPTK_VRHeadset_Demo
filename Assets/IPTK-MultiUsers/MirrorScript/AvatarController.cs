using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class AvatarController : NetworkBehaviour
{
    public Transform leftHandTarget;
    public Transform rightHandTarget;
    public Transform leftHandAnchor;
    public Transform rightHandAnchor;
    public Transform headTarget;
    public Transform headAnchor;
    //public Transform neckTransform;

    private Vector3 initialHeadPosition;

    // Start is called before the first frame update
    void Start()
    {
        if (headTarget != null)
        {
            initialHeadPosition = headTarget.localPosition;
        }

        if (XRSettings.isDeviceActive)
        {
            UpdateAvatarJointsPos();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            if (XRSettings.isDeviceActive) 
            { 
                UpdateAvatarJointsPos();
            }
        }
    }

    private void UpdateAvatarJointsPos()
    {
        // 同步左手位置和旋转
        if (leftHandTarget != null && leftHandAnchor != null)
        {
            leftHandTarget.position = leftHandAnchor.position;
            leftHandTarget.rotation = leftHandAnchor.rotation;
        }

        // 同步右手位置和旋转
        if (rightHandTarget != null && rightHandAnchor != null)
        {
            rightHandTarget.position = rightHandAnchor.position;
            rightHandTarget.rotation = rightHandAnchor.rotation;
        }

        // 同步头部位置和旋转
        if (headTarget != null && headAnchor != null)
        {
            headTarget.localPosition = initialHeadPosition;

            headTarget.rotation = headAnchor.rotation;
        }
    }
}
