using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class VRHeadsetController : MonoBehaviour, IPlayerController
{
    private Player player;
    private OVRCameraRig cameraRig; // VR camera rig
    private CharacterController characterController;

    [SerializeField]
    private LineRenderer laserLineRenderer;

    [SerializeField]
    private Transform rightHandAnchor;

    [SerializeField]
    private float laserMaxLength = 99999.0f;

    [SerializeField]
    private float teleportFadeDuration = 0.5f; // For fading out/in during teleport

    [SerializeField]
    private float speed = 500.0f;

    [SerializeField]
    private float gravity = 1000.0f;

    private Vector3 moveDirection = Vector3.zero;

    public void Initialize(Player player)
    {
        this.player = player;

        cameraRig = GetComponentInChildren<OVRCameraRig>();
        if (cameraRig == null)
        {
            Debug.LogError("OVRCameraRig not found in player prefab.");
        }

        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("CharacterController not found in player prefab.");
        }

        if (rightHandAnchor == null)
        {
            rightHandAnchor = cameraRig.transform.Find("TrackingSpace/RightHandAnchor");  // 获取右手控制器的锚点
        }

        if (laserLineRenderer == null)
        {
            laserLineRenderer = rightHandAnchor.gameObject.AddComponent<LineRenderer>();
            laserLineRenderer.startWidth = 0.1f;
            laserLineRenderer.endWidth = 0.1f;
            laserLineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            laserLineRenderer.material.color = Color.red;
        }
    }

    public void HandleInput(Camera playerCamera)
    {

        HandleLaserPointer();
        if (player.movementType == MovementType.FreeRoaming)
        {
            HandleFreeRoaming();
        }

        if (player.userType == PlayerType.Presenter)
        {
            HandleSceneChange();
        }
    }

    private void HandleLaserPointer()
    {
        // 设置激光起点和方向
        Vector3 laserStart = rightHandAnchor.position;
        Vector3 laserDirection = rightHandAnchor.forward;

        Ray ray = new Ray(laserStart, laserDirection);
        RaycastHit hit;
        Vector3 laserEnd = laserStart + (laserDirection * laserMaxLength);

        if (Physics.Raycast(ray, out hit, laserMaxLength))
        {
            laserEnd = hit.point;

            // 检查被击中的物体是否继承自 Interactable
            Interactable interactable = hit.collider.GetComponent<Interactable>();
            TeleportPoint teleportPoint = hit.collider.GetComponent<TeleportPoint>();

            // 只有玩家的移动方式是 Teleport 时，才允许传送
            if (teleportPoint != null && player.movementType == MovementType.Teleport)
            {
                // 设置点击的 Player
                teleportPoint.SetPlayer(player);

                // 如果用户按下触发按钮，触发交互
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    teleportPoint.Interact();  // 调用传送逻辑
                }
            }
            else if (interactable != null)
            {
                // 处理其他可交互物体的交互
                if (OVRInput.GetDown(OVRInput.Button.SecondaryIndexTrigger))
                {
                    interactable.Interact();
                }
            }
        }

        // 更新激光的 LineRenderer 位置
        laserLineRenderer.SetPosition(0, laserStart);
        laserLineRenderer.SetPosition(1, laserEnd);
    }

    private void HandleFreeRoaming()
    {
        // 获取 VR 控制器的摇杆输入
        Vector2 secondaryThumbstick = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // 构建水平移动方向
        Vector3 moveDirection = new Vector3(secondaryThumbstick.x, 0, secondaryThumbstick.y);

        // 将移动方向与摄像机的方向对齐
        moveDirection = cameraRig.centerEyeAnchor.transform.TransformDirection(moveDirection);
        moveDirection.y = 0;  // 确保不会向上或向下移动

        // 如果角色在地面上，处理水平移动
        if (characterController.isGrounded)
        {
            // 将水平移动应用到 moveDirection
            this.moveDirection = moveDirection * speed;
        }
        // 处理重力
        this.moveDirection.y -= gravity * Time.deltaTime;

        // 使用 CharacterController 移动角色
        characterController.Move(this.moveDirection * Time.deltaTime);
    }


    private void HandleTeleport(Camera playerCamera)
    {
        TeleportPointManager teleportPointManager = FindObjectOfType<TeleportPointManager>();
        if (teleportPointManager == null)
        {
            return;
        }

        // Handle teleportation, likely based on the VR controller's raycast
        Vector3? teleportPosition = FindObjectOfType<TeleportPointManager>().CheckForTeleportPointClick(playerCamera);
        if (teleportPosition.HasValue)
        {
            player.RequestTeleport(teleportPosition.Value);
        }
    }

    private void HandleSceneChange()
    {
        if (OVRInput.GetDown(OVRInput.Button.One) && player._currentSessionIndex < player._presentationData.Timeline.transitionData.Length - 1)
        {
            player.CmdChangeScene(player._presentationData.Timeline.transitionData[player._currentSessionIndex].nextSession.sceneName);
        }
        else if (OVRInput.GetDown(OVRInput.Button.Two) && player._currentSessionIndex > 0)
        {
            player.CmdChangeScene(player._presentationData.Timeline.transitionData[player._currentSessionIndex - 1].lastSession.sceneName);
        }
    }
}
