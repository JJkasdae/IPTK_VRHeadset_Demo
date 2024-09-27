using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;  // 引入 Mirror 框架
using UnityEngine.UI;  // 引入 UI 组件

public class ImageDisplay : NetworkBehaviour
{
    public MeshRenderer meshRenderer; // 用来显示图片的3D对象上的MeshRenderer
    public List<Texture2D> images;    // 存储所有图片的列表
    [SyncVar(hook = nameof(OnCurrentIndexChanged))]
    private int currentIndex = 0;     // 当前显示图片的索引

    public override void OnStartServer()
    {
        base.OnStartServer();
        RpcShowImage(currentIndex);  // 让服务器的客户端也看到当前图片
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("Client is starting, syncing with server...");
        CmdRequestCurrentImage();
    }

    private void OnCurrentIndexChanged(int oldIndex, int newIndex)
    {
        ShowImage(newIndex);  // 更新显示当前图片
    }

    // 显示当前索引的图片
    public void ShowImage(int index)
    {
        if (index >= 0 && index < images.Count)
        {
            meshRenderer.material.mainTexture = images[index];
        }
    }

    // 本地客户端点击按钮切换到下一张图片
    public void NextImage()
    {
        CmdNextImage();  // 发送命令到服务器
    }

    // 本地客户端点击按钮切换到上一张图片
    public void PreviousImage()
    {
        CmdPreviousImage();  // 发送命令到服务器
        
    }

    // 切换到下一张图片的命令，从客户端发送到服务器
    [Command(requiresAuthority = false)]
    public void CmdNextImage()
    {
        currentIndex++;
        if (currentIndex >= images.Count)
        {
            currentIndex = 0;  // 如果到达最后一张，循环回到第一张
        }

        //RpcShowImage(currentIndex);  // 同步到所有客户端
    }

    // 切换到上一张图片的命令，从客户端发送到服务器
    [Command(requiresAuthority = false)]
    public void CmdPreviousImage()
    {
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = images.Count - 1;  // 如果到达第一张，循环回到最后一张
        }

        //RpcShowImage(currentIndex);  // 同步到所有客户端
    }

    [Command(requiresAuthority = false)]
    public void CmdRequestCurrentImage()
    {
        // 使用 Rpc 向所有客户端同步当前图片
        RpcShowImage(currentIndex);
    }

    // 客户端更新图片显示
    [ClientRpc]
    public void RpcShowImage(int index)
    {
        ShowImage(index);  // 在所有客户端同步显示当前图片
    }
}
