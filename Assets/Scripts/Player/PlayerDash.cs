using Mirror;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerRelativeCameraMovementController))]
public class PlayerDash : NetworkBehaviour
{
    public bool IsDashActive => _dashActive;

    private const int MouseButtonLMB = 0;

    [SerializeField]
    private float _dashForce = 3.0f;

    [SerializeField]
    private float _dashDuration = 0.5f;

    private PlayerRelativeCameraMovementController _movementController;

    [SyncVar]
    private bool _dashActive = false;

    private void Awake()
    {
        _movementController = GetComponent<PlayerRelativeCameraMovementController>();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        ReadDashInput();
    }

    private void ReadDashInput()
    {
        if (Input.GetMouseButtonDown(MouseButtonLMB))
        {
            ActivateDash();
        }
    }
    
    private void ActivateDash()
    {
        if (_dashActive)
            return;

        _movementController.ApplyImpulse(_dashForce);

        StartCoroutine(ResetDash(_dashDuration));
        CmdActivateDash();
    }

    [Command]
    private void CmdActivateDash()
    {
        _dashActive = true;
    }

    [Command]
    private void CmdResetDash()
    {
        _dashActive = false;
    }

    private IEnumerator ResetDash(float delay)
    {
        yield return new WaitForSeconds(delay);

        _movementController.ResetImpulse();
        CmdResetDash();
    }
}
