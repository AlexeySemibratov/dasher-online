using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerScore))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerDash))]
public class Player : NetworkBehaviour
{
    private PlayerScore _score;
    private PlayerHealth _health;
    private PlayerDash _dash;

    [SyncVar]
    private string _name;

    [Server]
    public void Init(string name)
    {
        _name = name;
    }

    public bool IsInvinsible()
    {
        return _health.IsInvinsible;
    }

    public void TakeHit()
    {
        _health.Hit();
    }

    public override void OnStartClient()
    {
        base.OnStartServer();

        MainCanvas.Scoreboard.BindPlayerScore(_name, _score);
    }

    public override void OnStartLocalPlayer()
    {
        MainCanvas.Instance.Activate();
    }

    public override void OnStopLocalPlayer()
    {
        MainCanvas.Instance.Deactivate();
    }

    public override void OnStopClient()
    {
        MainCanvas.Scoreboard.UnbindPlayerScore(_name);
    }

    private void Awake()
    {
        _score = GetComponent<PlayerScore>();
        _health = GetComponent<PlayerHealth>();
        _dash = GetComponent<PlayerDash>();
    }

    [ServerCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            HitTarget(player);
        }
    }

    private void HitTarget(Player target)
    {
        if (_dash.IsDashActive && target.IsInvinsible() == false)
        {
            target.TakeHit();
            _score.IncrementScore();
        }
    }

    [ClientCallback]
    private void OnApplicationFocus(bool focus)
    {
        if (!isLocalPlayer)
            return;

        return; // TODO FIX
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
