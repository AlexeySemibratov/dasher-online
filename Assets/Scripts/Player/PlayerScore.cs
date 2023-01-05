using Mirror;
using System;

public class PlayerScore : NetworkBehaviour
{
    public event Action<int> ScoreChanged;

    public int Score => score;

    [SyncVar(hook = nameof(OnScoreChanged))]
    private int score = 0;

    public void IncrementScore()
    {
        score++;
    }

    private void OnScoreChanged(int oldValue, int newValue)
    {
        ScoreChanged?.Invoke(newValue);
    }
}
