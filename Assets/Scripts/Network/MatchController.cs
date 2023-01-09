using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchController : NetworkBehaviour
{
    [SerializeField]
    private int _scoresToWin = 5;

    [SerializeField]
    private float _restartTime = 5.0f;

    [SerializeField]
    private NetworkManagerExt _networkManager;

    private IEnumerable<Player> _players;

    [Server]
    public void UpdatePlayers(IEnumerable<Player> players)
    {
        _players = players;
        foreach (Player player in players)
        {
            var scoreHandler = new PlayerScoreRegister(player);
            scoreHandler.RegisterScoreHandler(OnPlayerScoreChanged);
        }
    }

    [Server]
    private void OnPlayerScoreChanged(Player player, int score)
    {
        if (score >= _scoresToWin)
        {
            RegisterWinner(player);
        }
    }

    [Server]
    private void RegisterWinner(Player player)
    {
        _networkManager.DisablePlayersMovement();
        StartCoroutine(RestartMatch());
        RpcShowWinnerDialog(player.PlayerName, (int)_restartTime);
    }

    [Server]
    private IEnumerator RestartMatch()
    {
        yield return new WaitForSeconds(_restartTime);

        ResetScores();
        _networkManager.RespawnPlayers();
        _networkManager.EnablePlayersMovement();
        RpcHideWinnerDialog();
    }

    [Server]
    private void ResetScores()
    {
        foreach (var player in _players)
        {
            player.PlayerScore.Reset();
        }
    }

    [ClientRpc]
    private void RpcShowWinnerDialog(string winnerName, int restartTime)
    {
        MainCanvas.WinnerPanel.Show(winnerName, restartTime);
    }

    [ClientRpc]
    private void RpcHideWinnerDialog()
    {
        MainCanvas.WinnerPanel.Hide();
    }

    private class PlayerScoreRegister
    {
        public Player Player { get; private set; }

        private Action<Player, int> _playerScoreChangedListener;

        public PlayerScoreRegister()
        {
        }

        public PlayerScoreRegister(Player player)
        {
            Player = player;
        }

        public void RegisterScoreHandler(Action<Player, int> scoreHandler)
        {
            _playerScoreChangedListener = scoreHandler;
            Player.PlayerScore.ScoreChanged += PlayerScoreChanged;
        }

        private void PlayerScoreChanged(int score)
        {
            _playerScoreChangedListener.Invoke(Player, score);
        }
    }
}
