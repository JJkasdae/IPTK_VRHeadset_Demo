using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : Interactable
{
    private Renderer objectRenderer;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
    }

    public override void Interact()
    {
        base.Interact();
        // 发送交互请求到服务器
        CmdRequestChangeColor();
    }

    [Command(requiresAuthority = false)]
    void CmdRequestChangeColor()
    {
        // 在服务器上执行颜色变化，并同步到所有客户端
        Color newColor = new Color(Random.value, Random.value, Random.value);
        RpcChangeColor(newColor);
    }

    [ClientRpc]
    void RpcChangeColor(Color color)
    {
        objectRenderer.material.color = color;
    }
}
