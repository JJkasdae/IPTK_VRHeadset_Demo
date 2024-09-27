using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    FreeRoaming,
    NoMovement,
    Teleport
}

public class SpawnPointAttribute : NetworkBehaviour
{

    [SyncVar]
    public bool isOccupied = false;

    [SyncVar]
    public MovementType movementType;

    [SyncVar]
    public PlayerType playerType;

    [Server]
    public void Occupy()
    {
        isOccupied = true;
    }

    public void SetMovementType(MovementType type)
    {
        movementType = type;
    }

    public void SetPlayerType(PlayerType type)
    {
        playerType = type;
    }

}
