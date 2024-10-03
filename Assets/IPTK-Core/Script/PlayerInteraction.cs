using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    private InputAction clickAction;

    // Start is called before the first frame update
    void Start()
    {
        clickAction = new InputAction(binding: "<Mouse>/leftButton");
        clickAction.performed += OnMouseClick;
        clickAction.Enable();
    }

    private void OnDisable()
    {
        clickAction.Disable();
    }

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.transform.gameObject;

            Interactable interactable = hitObject.GetComponent<Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
