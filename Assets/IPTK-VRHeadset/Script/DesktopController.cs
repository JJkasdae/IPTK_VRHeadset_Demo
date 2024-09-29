using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            if (Input.GetKeyDown(KeyCode.Space))
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
        

        if (player.userType == PlayerType.Presenter)
        {
            HandleSceneChange();
            AttentionUIMessage();
            HandlePromptDisplay();
        }
    }

    private void FreeRoamingMovement()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        // 使用WASD控制移动，忽略方向键
        if (Input.GetKey(KeyCode.W))
        {
            moveVertical = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            moveVertical = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
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
        if (Input.GetKey(KeyCode.J))
        {
            transform.Rotate(Vector3.up * -rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.L))
        {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.I))
        {
            transform.Rotate(Vector3.left * rotationSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.K))
        {
            transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime);
        }
        
    }

    private void TeleportMovement(Camera playerCamera)
    {
        Debug.Log("Desktop teleport");
        Vector3? teleportPosition = FindObjectOfType<TeleportPointManager>().CheckForTeleportPointClick(playerCamera);
        if (teleportPosition.HasValue)
        {
            player.RequestTeleport(teleportPosition.Value);
        }
    }

    private void HandleSceneChange()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow) && player._currentSessionIndex < player._presentationData.Timeline.transitionData.Length - 1)
        {
            player.CmdChangeScene(player._presentationData.Timeline.transitionData[player._currentSessionIndex].nextSession.sceneName);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && player._currentSessionIndex > 0)
        {
            player.CmdChangeScene(player._presentationData.Timeline.transitionData[player._currentSessionIndex - 1].lastSession.sceneName);
        }
    }

    private void HandlePromptDisplay()
    {
        // Trigger the display prompt using the left controller's button
        if (Input.GetKeyDown(KeyCode.O)) // Example: Left controller button press
        {
            if (displayPrompt != null)
            {
                displayPrompt.TogglePrompt(); // Call the method to toggle the prompt's visibility
            }
        }
    }

    private void AttentionUIMessage()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            print("Click on Prompt UI");
            player.CmdToggleAudienceReminder();
        }
    }

    //private void HandleTeleportInput()
    //{
    //    if (Input.GetMouseButtonDown(1))
    //    {
    //        print("Right clicking");
    //        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
    //        RaycastHit hit;

    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
    //        {
    //            print(hit.transform.gameObject.name);
    //            print("In the if statement.");
    //            player.RequestTeleport(hit.point);
    //        }
    //    }
    //}

}
