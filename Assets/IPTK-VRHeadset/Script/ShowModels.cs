using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowModels : NetworkBehaviour
{
    private int currentIndex = -1; // 当前显示的星球的索引，-1 表示没有显示任何星球

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
        {
            // 初始化时隐藏所有星球
            HideAllChildren();
        }
    }

    // 隐藏所有的子物体
    void HideAllChildren()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    // 切换到下一个星球
    [Server]
    public void ShowNextPlanet()
    {
        // 隐藏当前星球
        if (currentIndex >= 0)
        {
            Transform currentPlanet = transform.GetChild(currentIndex);
            currentPlanet.gameObject.SetActive(false);
        }

        // 更新索引到下一个星球
        currentIndex = (currentIndex + 1) % transform.childCount; // 循环到第一个
        Transform nextPlanet = transform.GetChild(currentIndex);

        // 显示下一个星球并同步到客户端
        nextPlanet.gameObject.SetActive(true);
        RpcSyncPlanet(currentIndex);
    }

    // 在客户端同步显示星球
    [ClientRpc]
    void RpcSyncPlanet(int index)
    {
        HideAllChildren(); // 确保客户端其他星球被隐藏
        Transform planet = transform.GetChild(index);
        planet.gameObject.SetActive(true); // 显示当前星球
    }

    // 你可以通过Command调用该函数，客户端请求服务器切换到下一个星球
    [Command]
    public void CmdNextPlanet()
    {
        ShowNextPlanet(); // 服务器执行并同步
    }
}
