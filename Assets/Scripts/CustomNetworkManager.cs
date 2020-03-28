﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Linq;

public class CustomNetworkManager : NetworkManager
{
    public delegate void ClientConnectedToServer(NetworkConnection conn);
    public static ClientConnectedToServer OnClientConnectedToServer;

    public delegate void ClientDisconnectedFromServer(NetworkConnection conn);
    public static ClientDisconnectedFromServer OnClientDisconnectedFromServer;

    public delegate void PlayerAddedToServer(Player newPlayer);
    public static PlayerAddedToServer OnPlayerAddedToServer;

    public delegate void PlayerRemovedFromServer(Player player);
    public static PlayerRemovedFromServer OnPlayerRemovedFromServer;

    public static CustomNetworkManager Instance { get; private set; }

    public List<Player> ConnectedPlayers { get; private set; }

    public override void Awake()
    {
        base.Awake();
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ConnectedPlayers = new List<Player>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        OnClientConnectedToServer?.Invoke(conn);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);
        OnClientDisconnectedFromServer?.Invoke(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        Debug.Log("Player added");
        GameObject createdPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Player player = createdPlayer.GetComponent<Player>();
        player.Connection = conn;
        ConnectedPlayers.Add(player);
        player.debugInfo = "Debug - " + ConnectedPlayers.Count();
        NetworkServer.AddPlayerForConnection(conn, createdPlayer);
        OnPlayerAddedToServer?.Invoke(player);
    }

    public override void OnServerRemovePlayer(NetworkConnection conn, NetworkIdentity player)
    {
        base.OnServerRemovePlayer(conn, player);
        Player exitingPlayer = GetPlayer(conn);
        ConnectedPlayers.Remove(exitingPlayer);
        OnPlayerRemovedFromServer?.Invoke(exitingPlayer);
    }

    public Player GetPlayer(NetworkConnection connection)
    {
        return ConnectedPlayers.First(x => x.Connection == connection);
    }

    public Player GetPlayer(NetworkIdentity identity)
    {
        return ConnectedPlayers.First(x => x.netIdentity == identity);
    }
}
