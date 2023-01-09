using TMPro;
using UnityEngine;

public class WinnerPanelView : MonoBehaviour
{
    private const string NameText = "Winner is {0}";
    private const string RestartText = "Restart after {0} sec";

    [SerializeField]
    private TextMeshProUGUI _winnerText;

    [SerializeField]
    private TextMeshProUGUI _restartText;

    public void Show(string name, int time)
    {
        _winnerText.text = string.Format(NameText, name);
        _restartText.text = string.Format(RestartText, time);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
