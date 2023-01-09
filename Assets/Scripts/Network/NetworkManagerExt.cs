using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NetworkManagerExt : NetworkManager
{
    [SerializeField]
    private MatchController _matchController;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Player player = CreatePlayerObject();

        InitPlayer(player, CreateName(conn));
        NetworkServer.AddPlayerForConnection(conn, player.gameObject);

        _matchController.UpdatePlayers(GetActivePlayers());
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnServerDisconnect(conn);
        _matchController.UpdatePlayers(GetActivePlayers());
    }

    public void RespawnPlayers()
    {
        foreach (Player player in GetActivePlayers())
        {
            Vector3 position = GetStartPosition().position;
            player.Teleport(position);
        }
    }

    public void EnablePlayersMovement()
    {
        foreach (Player player in GetActivePlayers())
        {
            player.EnableMovement();
        }
    }

    public void DisablePlayersMovement()
    {
        foreach (Player player in GetActivePlayers())
        {
            player.DisableMovement();
        }
    }

    private IEnumerable<Player> GetActivePlayers() {
        return NetworkServer.connections.Values.Select(conn => conn.identity.gameObject.GetComponent<Player>());
    }

    private Player CreatePlayerObject()
    {
        Transform position = GetStartPosition();
        GameObject playerObj = Instantiate(playerPrefab, position.position, position.rotation);

        if (playerObj.TryGetComponent(out Player player))
        {
            return player;
        }
        else
        {
            throw new ArgumentException("Missing component Player when instantiate player on server.");
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
