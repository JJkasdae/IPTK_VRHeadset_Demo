using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // 如果鼠标悬停在 UI 元素上，不执行 3D 对象的交互
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        // 检测鼠标点击
        if (Input.GetMouseButtonDown(0)) // 0 表示左键点击
        {
            RaycastHit hit;
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

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
}
