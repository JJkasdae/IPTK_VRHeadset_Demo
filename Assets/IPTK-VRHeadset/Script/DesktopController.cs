using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DesktopController : MonoBehaviour, IPlayerController
{
    private Player player;
    private Camera playerCamera;
    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;


    [SerializeField]
    private float speed = 500.0f;

    [SerializeField]
    private float JumpSpeed = 300.0f;

    [SerializeField]
    private float gravity = 1000.0f;

    [SerializeField]
    private float rotationSpeed = 100f;

    private DisplayPrompt displayPrompt;

    private ShowModels showModels;

    private InputAction moveForwardAction;
    private InputAction moveBackwardAction;
    private InputAction moveLeftAction;
    private InputAction moveRightAction;
    private InputAction jumpAction;
    private InputAction nextPlanetAction;
    private InputAction rotateLeftAction;
    private InputAction rotateRightAction;
    private InputAction rotateUpAction;
    private InputAction rotateDownAction;
    private InputAction showPromptAction;
    private InputAction showAttentionUIAction;
    private InputAction nextSceneAction;
    private InputAction previousSceneAction;

    public void Initialize(Player player)
    {
        this.player = player;

        playerCamera = GetComponentInChildren<Camera>();
        if (playerCamera == null)
        {
            Debug.LogError("Camera not found in player prefab.");
        }
        
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("character controller cannot be found in player prefab");
        }

        // Initialize DisplayPrompt for the presenter
        if (player.userType == PlayerType.Presenter)
        {
            // Find the DisplayPrompt in the scene
            displayPrompt = FindObjectOfType<DisplayPrompt>();
        }

        showModels = FindObjectOfType<ShowModels>();

        moveForwardAction = new InputAction(binding: "<Keyboard>/w");
        moveBackwardAction = new InputAction(binding: "<Keyboard>/s");
        moveLeftAction = new InputAction(binding: "<Keyboard>/a");
        moveRightAction = new InputAction(binding: "<Keyboard>/d");
        jumpAction = new InputAction(binding: "<Keyboard>/space");
        nextPlanetAction = new InputAction(binding: "<Keyboard>/n");
        rotateLeftAction = new InputAction(binding: "<Keyboard>/j");
        rotateRightAction = new InputAction(binding: "<Keyboard>/l");
        rotateUpAction = new InputAction(binding: "<Keyboard>/k");
        rotateDownAction = new InputAction(binding: "<Keyboard>/i");
        showPromptAction = new InputAction(binding: "<Keyboard>/o");
        showAttentionUIAction = new InputAction(binding: "<Keyboard>/u");
        nextSceneAction = new InputAction(binding: "<Keyboard>/rightArrow");
        previousSceneAction = new InputAction(binding: "<Keyboard>/leftArrow");

        nextPlanetAction.performed += ShowModels;
        showPromptAction.performed += HandlePromptDisplay;
        showAttentionUIAction.performed += AttentionUIMessage;
        nextSceneAction.performed += NextScenePressed;
        previousSceneAction.performed += LastScenePressed;

        OnEnable();


    }

    private void OnEnable()
    {
        if (moveForwardAction != null && moveBackwardAction != null && moveLeftAction != null && moveRightAction != null &&
            jumpAction != null && rotateLeftAction != null && rotateRightAction != null && rotateUpAction != null &&
            rotateDownAction != null && nextPlanetAction != null && showPromptAction != null && showAttentionUIAction != null &&
            nextSceneAction != null && previousSceneAction != null)
        {
            moveForwardAction.Enable();
            moveBackwardAction.Enable();
            moveLeftAction.Enable();
            moveRightAction.Enable();
            jumpAction.Enable();
            rotateLeftAction.Enable();
            rotateRightAction.Enable();
            rotateUpAction.Enable();
            rotateDownAction.Enable();
            nextPlanetAction.Enable();
            showPromptAction.Enable();
            showAttentionUIAction.Enable();
            nextSceneAction.Enable();
            previousSceneAction.Enable();
        }
    }

    private void OnDisable()
    {
        moveForwardAction.Disable();
        moveBackwardAction.Disable();
        moveLeftAction.Disable();
        moveRightAction.Disable();
        jumpAction.Disable();
        rotateLeftAction.Disable();
        rotateRightAction.Disable();
        rotateUpAction.Disable();
        rotateDownAction.Disable();
        nextPlanetAction.Disable();
        showPromptAction.Disable();
        showAttentionUIAction.Disable();
        nextSceneAction.Disable();
        previousSceneAction.Disable();
    }

    public void HandleInput(Camera playerCamera)
    {
        if (player.movementType == MovementType.FreeRoaming)
        {
            if (characterController.isGrounded)
            {
                FreeRoamingMovement();
                moveDirection.y = 0f;
            }

            if (jumpAction.triggered)
            {
                moveDirection.y = JumpSpeed;
            }

            moveDirection.y -= gravity * Time.deltaTime;
            characterController.Move(moveDirection * Time.deltaTime);
        }
        else if (player.movementType == MovementType.Teleport)
        {
            //print("Player movement is teleport");
            TeleportMovement(playerCamera);
        }

        PlayerRotation();
    }

    private void FreeRoamingMovement()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        // 使用WASD控制移动，忽略方向键
        if (moveForwardAction.ReadValue<float>() > 0)
        {
            moveVertical = 1f;
        }
        else if (moveBackwardAction.ReadValue<float>() > 0)
        {
            moveVertical = -1f;
        }

        if (moveLeftAction.ReadValue<float>() > 0)
        {
            moveHorizontal = -1f;
        }
        else if (moveRightAction.ReadValue<float>() > 0)
        {
            moveHorizontal = 1f;
        }

        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        movement = playerCamera.transform.TransformDirection(movement);
        movement *= speed;

        characterController.Move(movement * Time.deltaTime);
    }

    private void PlayerRotation()
    {
        // 使用Q和E键进行左右旋转
        if (rotateLeftAction.ReadValue<float>() > 0)
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
        if (rotateRightAction.ReadValue<float>() > 0)
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (rotateDownAction.ReadValue<float>() > 0)
        {
            transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
        }
        if (rotateUpAction.ReadValue<float>() > 0)
        {
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
        
    }

    private void TeleportMovement(Camera playerCamera)
    {
        //Debug.Log("Desktop teleport");
        Vector3? teleportPosition = FindObjectOfType<TeleportPointManager>().CheckForTeleportPointClick(playerCamera);
        if (teleportPosition.HasValue)
        {
            player.RequestTeleport(teleportPosition.Value);
        }
    }

    private void NextScenePressed(InputAction.CallbackContext context)
    {
        if (player.userType == PlayerType.Presenter && player._currentSessionIndex < player._presentationData.Timeline.transitionData.Length - 1)
        {
            player.CmdChangeScene(player._presentationData.Timeline.transitionData[player._currentSessionIndex].nextSession.sceneName);
        }
    }

    private void LastScenePressed(InputAction.CallbackContext context)
    {
        if (player.userType == PlayerType.Presenter && player._currentSessionIndex > 0)
        {
            player.CmdChangeScene(player._presentationData.Timeline.transitionData[player._currentSessionIndex - 1].lastSession.sceneName);
        }
    }

    private void HandlePromptDisplay(InputAction.CallbackContext context)
    {
        if (player.userType == PlayerType.Presenter)
        {
            // Trigger the display prompt using the left controller's button
            if (displayPrompt != null)
            {
                displayPrompt.TogglePrompt(); // Call the method to toggle the prompt's visibility
            }
        }
    }

    private void AttentionUIMessage(InputAction.CallbackContext context)
    {
        if (player.userType == PlayerType.Presenter)
        {
            print("Click on Prompt UI");
            player.CmdToggleAudienceReminder();
        }
    }

    private void ShowModels(InputAction.CallbackContext context)
    {
        if (player.userType == PlayerType.Presenter)
        {
            if (player.isServer)
            {
                showModels.ShowNextPlanet(); // 服务器直接切换星球
            }
            else if (player.isLocalPlayer)
            {
                // 如果客户端没有权限，通过 Command 让服务器切换
                showModels.CmdNextPlanet();
            }
        }
    }

}
