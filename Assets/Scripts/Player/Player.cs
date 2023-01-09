using Mirror;
using UnityEngine;

[RequireComponent(typeof(PlayerScore))]
[RequireComponent(typeof(PlayerHealth))]
[RequireComponent(typeof(PlayerDash))]
[RequireComponent(typeof(PlayerRelativeCameraMovementController))]
public class Player : NetworkBehaviour
{
    // Unique player name
    public string PlayerName => _name;

    public PlayerScore PlayerScore => _score;

    private PlayerScore _score;
    private PlayerHealth _health;
    private PlayerDash _dash;
    private PlayerRelativeCameraMovementController _movementController;

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

    [Server]
    public void TakeHit()
    {
        _health.Hit();
    }

    [Server]
    public void Teleport(Vector3 destination)
    {
        _movementController.Teleport(destination);
    }

    [Server]
    public void EnableMovement()
    {
        _movementController.RpcEnable();
    }

    [Server]
    public void DisableMovement()
    {
        _movementController.RpcDisable();
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
        _movementController = GetComponent<PlayerRelativeCameraMovementController>();
    }

    [ClientCallback]
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Player player))
        {
            HitTarget(player);
        }
    }

    [Command]
    private void HitTarget(Player target)
    {
        if (_dash.IsDashActive && target.IsInvinsible() == false)
        {
            target.TakeHit();
            _score.IncrementScore();
        }
    }
}
