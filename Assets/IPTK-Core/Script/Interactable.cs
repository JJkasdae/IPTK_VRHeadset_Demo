using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : NetworkBehaviour
{
    public virtual void Interact()
    {
        // 这里可以定义通用的交互行为，比如打印调试信息
        Debug.Log("Interacting with " + gameObject.name);
    }
}
