using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance { get; private set; }

    public static PlayerScoreboardView Scoreboard => Instance.GetScoreboard();
    public static WinnerPanelView WinnerPanel => Instance.GetWinnerPanel();

    [SerializeField]
    private GameObject _rootView;

    [SerializeField]
    private PlayerScoreboardView _scoreboardView;

    [SerializeField]
    private WinnerPanelView _winnerPanel;

    public void Activate()
    {
        _rootView.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _rootView.gameObject.SetActive(false);
    }

    public PlayerScoreboardView GetScoreboard() => _scoreboardView;
    public WinnerPanelView GetWinnerPanel() => _winnerPanel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Trying to create another instance of Main Canvas.");
            Destroy(gameObject);
        }
    }
}
