using Mirror;
using System;
using UnityEngine;

public class NetworkManagerExt : NetworkManager
{
    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Transform startPos = GetStartPosition();
        GameObject playerObj = startPos != null
            ? Instantiate(playerPrefab, startPos.position, startPos.rotation)
            : Instantiate(playerPrefab);

        if (playerObj.TryGetComponent(out Player player))
        {
            InitPlayer(player, CreateName(conn));
            NetworkServer.AddPlayerForConnection(conn, playerObj);
        }
        else
        {
            Destroy(playerObj);
            throw new ArgumentException("Fail when try to spawn player. Player prefab must have Player component.");
        }
    }

    private void InitPlayer(Player player, string name)
    {
        player.Init(name);
    }

    private string CreateName(NetworkConnectionToClient conn)
    {
        return $"Player [{conn.connectionId}]";
    }
}
