using TMPro;
using UnityEngine;

public class PlayerScoreItemView : MonoBehaviour
{
    private const string PlayerScoreText = "{0}: {1}";

    [SerializeField]
    private TextMeshProUGUI _text;

    private string _playerName;

    public void SetData(string playerName, PlayerScore playerScore)
    {
        SetupView(playerName, playerScore);
    }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void SetupView(string playerName, PlayerScore playerScore)
    {
        _playerName = playerName;
        SetScoreText(playerScore.Score);
        playerScore.ScoreChanged += OnPlayerScoreChanged;
    }

    private void OnPlayerScoreChanged(int score)
    {
        SetScoreText(score);
    }

    private void SetScoreText(int score)
    {
        _text.text = string.Format(PlayerScoreText, _playerName, score);
    }
}
