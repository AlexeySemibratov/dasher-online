using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    public static MainCanvas Instance { get; private set; }

    public static PlayerScoreboardView Scoreboard => Instance.GetScoreboard();

    [SerializeField]
    private GameObject _rootView;

    [SerializeField]
    private PlayerScoreboardView _scoreboardView;

    public void Activate()
    {
        _rootView.gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        _rootView.gameObject.SetActive(false);
    }

    public PlayerScoreboardView GetScoreboard() => _scoreboardView;

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
