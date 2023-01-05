using System.Collections.Generic;
using UnityEngine;

public class PlayerScoreboardView : MonoBehaviour
{
    [SerializeField]
    private PlayerScoreItemView _scoreboardItemPrefab;

    private Dictionary<string, PlayerScoreItemView> _items = new Dictionary<string, PlayerScoreItemView>();

    public void BindPlayerScore(string playerName, PlayerScore score)
    {
        AddPlayerScoreItem(playerName, score);
    }

    public void UnbindPlayerScore(string playerName)
    {
        RemoveItem(playerName);
    }

    private void AddPlayerScoreItem(string playerName, PlayerScore playerScore)
    {
        PlayerScoreItemView playerScoreItem = Instantiate(_scoreboardItemPrefab, transform);
        playerScoreItem.SetData(playerName, playerScore);

        _items[playerName] = playerScoreItem;
    }


    private void RemoveItem(string playerName)
    {
        if (_items.TryGetValue(playerName, out PlayerScoreItemView item))
        {
            Destroy(item.gameObject);
            _items.Remove(playerName);
        }
    }
}
