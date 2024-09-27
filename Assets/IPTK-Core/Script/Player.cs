using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.XR;
using UnityEngine.AI;
using TMPro;

public enum PlayerType
{
    Presenter,
    Audience
}

public class Player : NetworkBehaviour
{
    [SyncVar]
    public PlayerType userType = PlayerType.Audience;

    [SyncVar]
    public MovementType movementType;

    public PresentationData _presentationData;

    public GameObject attentionUIPrefab;
    private GameObject attentionUIInstance;

    private IPlayerController controller;
    private Camera playerCamera;
    private bool isFull; // Check whether all spawn points are used
    public int _currentSessionIndex = 0;
    private string _currentSceneName;

    void Start()
    {
        if (isLocalPlayer)
        {
            CmdRequestSpawnPoint(); // 请求服务器分配一个 spawn point
            DetectInputDevice();

            initializeSceneData();
            //GameObject networkConnectionUI = GameObject.Find("Network Connection UI");
            //if (networkConnectionUI != null)
            //{
            //    networkConnectionUI.SetActive(false);
            //}
        }
        else
        {
            GameObject avataHead = this.transform.GetChild(0).gameObject;
            avataHead.transform.GetChild(0).gameObject.SetActive(false);
        }

        if (isLocalPlayer && userType == PlayerType.Audience)
        {
            CreateAttentionUI();
        }
    }

    void DetectInputDevice()
    {
        GameObject DesktopCameraParent = this.transform.Find("DesktopCamera").gameObject;
        GameObject VRHeadsetCameraParent = this.transform.Find("VRHeadsetCamera").gameObject;
        if (XRSettings.isDeviceActive) // 注意测试阶段为true，实际阶段
        {
            Debug.Log("VR Headset is active");

            VRHeadsetCameraParent.SetActive(true);
            DesktopCameraParent.SetActive(false);
            GameObject VRCamera = this.transform.Find("VRHeadsetCamera/OVRCameraRig/TrackingSpace/CenterEyeAnchor").gameObject;
            playerCamera = VRCamera.GetComponent<Camera>();

            InitializeVRControls();
        }
        else
        {
            Debug.Log("Using Laptop/Desktop");
            DesktopCameraParent.SetActive(true);
            VRHeadsetCameraParent.SetActive(false);
            GameObject desktopCamera = this.transform.Find("DesktopCamera/CameraPrefab").gameObject;
            playerCamera = desktopCamera.GetComponent<Camera>();

            //initializeCamera(DesktopCamera);
            InitializeDesktopControls();
        }
    }

    void InitializeVRControls()
    {
        controller = gameObject.AddComponent<VRHeadsetController>() as IPlayerController;
        if (controller != null)
        {
            (controller as VRHeadsetController)?.Initialize(this);
            Debug.Log("VR Controls Initialized");
        }
        else
        {
            Debug.LogError("VRController could not be added or initialized.");
        }
    }

    void InitializeDesktopControls()
    {
        controller = gameObject.AddComponent<DesktopController>() as IPlayerController;
        if (controller != null)
        {
            (controller as DesktopController)?.Initialize(this);
            Debug.Log("Desktop Controls Initialized");
        }
        else
        {
            Debug.LogError("VRController could not be added or initialized.");
        }
    }

    [Command]
    void CmdRequestSpawnPoint(NetworkConnectionToClient conn = null)
    {
        // Check if a Presenter already exists on the server
        bool presenterExists = false;
        foreach (NetworkConnectionToClient connection in NetworkServer.connections.Values)
        {
            Player player = connection.identity.GetComponent<Player>();
            if (player != null && player.userType == PlayerType.Presenter)
            {
                presenterExists = true;
                break;
            }
        }

        // If no Presenter, set the current player as Presenter
        if (!presenterExists)
        {
            userType = PlayerType.Presenter;
            Debug.Log("Player is set as Presenter.");
        }

        string spawnPointsParentName = userType == PlayerType.Presenter ? "Presenter Spawn Points/SpawnPoints" : "Audience Spawn Points/SpawnPoints";
        GameObject spawnPointsParent = GameObject.Find(spawnPointsParentName);

        if (spawnPointsParent != null)
        {
            Transform[] spawnPoints = spawnPointsParent.GetComponentsInChildren<Transform>();

            isFull = false;

            foreach (Transform spawnPoint in spawnPoints)
            {
                SpawnPointAttribute spawnPointScript = spawnPoint.GetComponent<SpawnPointAttribute>();
                if (spawnPointScript != null && spawnPointScript.playerType == userType && !spawnPointScript.isOccupied)
                {
                    spawnPointScript.Occupy();

                    movementType = spawnPointScript.movementType;

                    Vector3 spawnPosition = spawnPoint.position;
                    spawnPosition.y += 50f;

                    RpcSetPlayerPosition(spawnPosition);
                    isFull = true;
                    break;
                }
            }

            if (!isFull)
            {
                Debug.LogWarning("No available spawn points! Disconnecting player.");
                TargetDisconnectPlayer(conn);
            }
        }
        else
        {
            Debug.LogError("SpawnPoints object not found in the scene!");
        }
    }

    [TargetRpc]
    void TargetDisconnectPlayer(NetworkConnection target)
    {
        Debug.Log("Disconnecting player due to lack of spawn points.");
        target.Disconnect();
    }


    [ClientRpc]
    void RpcSetPlayerPosition(Vector3 position)
    {
        transform.position = position;
    }

    void initializeSceneData()
    {
        _currentSceneName = SceneManager.GetActiveScene().name;

        Debug.Log(_presentationData.Timeline.transitionData.Length);

        for (int i = 0; i < _presentationData.Timeline.transitionData.Length; i++)
        {
            if (_presentationData.Timeline.transitionData[i].lastSession.sceneName == _currentSceneName)
            {
                _currentSessionIndex = i;
                break;
            }
        }

        Debug.Log(_presentationData.Timeline.transitionData[_currentSessionIndex].lastSession.sceneName);
    }

    [ServerCallback]
    void onServerSceneChanged(string sceneName)
    {
        // 为每个玩家重新分配一个 spawn point
        foreach (NetworkConnectionToClient conn in NetworkServer.connections.Values)
        {
            Player player = conn.identity.GetComponent<Player>();
            if (player != null)
            {
                player.CmdRequestSpawnPoint(conn);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        controller?.HandleInput(playerCamera);
    }

    private void CreateAttentionUI()
    {
        if (attentionUIPrefab != null && playerCamera != null)
        {
            attentionUIInstance = Instantiate(attentionUIPrefab, playerCamera.transform);
            attentionUIInstance.transform.localPosition = new Vector3(0, 0, 10);
            attentionUIInstance.SetActive(false);
        }
        else
        {
            Debug.LogError("AttentionUI Prefab or PlayerCamera is not assigned!");
        }
    }

    public override void OnStartLocalPlayer()
    {
        //base.OnStartLocalPlayer();
        GetComponent<Renderer>().material.color = Color.blue;
    }

    [Command]
    public void CmdChangeScene(string sceneName)
    {
        NetworkManager.singleton.ServerChangeScene(sceneName);
    }

    [Command]
    public void RequestTeleport(Vector3 pos)
    {
        RpcTeleportPlayer(pos);
    }

    [ClientRpc]
    private void RpcTeleportPlayer(Vector3 pos)
    {
        // 获取玩家底座的高度（通常是玩家脚的位置相对于玩家中心点的位置）
        float playerHeightOffset = transform.localScale.y / 2;

        // 计算新的传送位置，确保玩家底座与TeleportPoint中心对齐
        Vector3 correctedPosition = new Vector3(
            pos.x,
            pos.y + playerHeightOffset,
            pos.z
        );

        // 更新玩家的位置
        transform.position = correctedPosition;
    }

    [Command]
    public void CmdShowAttentionMessage()
    {
        RpcShowAttentionMessage();
        print("Call RPCShowAttentionMessage");
    }

    [ClientRpc]
    private void RpcShowAttentionMessage()
    {
        print("prompt UI");
        attentionUIInstance.SetActive(!attentionUIInstance.activeSelf);
    }
}
