using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPoint : Interactable
{

    private Player player;

    public void SetPlayer(Player newPlayer)
    {
        player = newPlayer;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public override void Interact()
    {
        // 当用户与传送点交互时，执行传送操作
        if (player != null)
        {
            Vector3 teleportPosition = GetPosition();
            player.RequestTeleport(teleportPosition);
            Debug.Log("Teleporting player to: " + teleportPosition);
        }
    }

}
