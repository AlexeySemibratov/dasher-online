using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCamera))]
[RequireComponent(typeof(NetworkTransform))]
public class PlayerRelativeCameraMovementController : NetworkBehaviour
{
    private const float DefaultImpulseMultiplier = 1.0f;

    [SerializeField]
    private float _movementSpeed = 2.0f;

    [SerializeField]
    private float _rotationSpeed = 360.0f;

    private PlayerCamera _playerCamera;
    private Transform _cameraTransform => _playerCamera.GetMainCameraTransform();
    private CharacterController _characterController;
    private NetworkTransform _networkTransform;

    private Vector3 _movementDirection = Vector3.zero;

    private float _impulseMultiplier = DefaultImpulseMultiplier;

    private bool _impulseActive = false;

    [ClientRpc]
    public void RpcEnable()
    {
        if (isLocalPlayer)
        {
            _characterController.enabled = true;
        }
    }

    [ClientRpc]
    public void RpcDisable()
    {
        if (isLocalPlayer)
        {
            _characterController.enabled = false;
        }
    }

    [Server]
    public void Teleport(Vector3 destination)
    {
        _networkTransform.RpcTeleport(destination);
    }

    public bool CanApplyImpulse()
    {
        return _movementDirection != Vector3.zero && _impulseActive == false;
    }

    public void ApplyImpulse(float value)
    {
        if (CanApplyImpulse() == false)
            return;

        ActivateImpulse(value);
    }

    public void ResetImpulse()
    {
        if (_impulseActive == false)
            return;

        ResetImpulseInternal();
    }

    public bool IsImpulseActive() => _impulseActive;

    public override void OnStartLocalPlayer()
    {
        _characterController.enabled = true;
    }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = GetComponent<PlayerCamera>();
        _networkTransform = GetComponent<NetworkTransform>();
        _characterController.enabled = false;
    }

    private void Update()
    {
        if (IsClientController() == false || _impulseActive)
            return;

        ReadMovementInput();
    }

    private void FixedUpdate()
    {
        if (IsClientController() == false)
            return;

        Move(_movementDirection);
        Rotate(_movementDirection);
    }

    private bool IsClientController() => isLocalPlayer && _characterController.enabled;

    private void ReadMovementInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (movementDirection.magnitude == 0)
        {
            _movementDirection = Vector3.zero;
            return;
        }

        _movementDirection = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;
    }

    private void Move(Vector3 direction)
    {
        float speedMultiplier = _movementSpeed * _impulseMultiplier;
        _characterController.Move(speedMultiplier * Time.fixedDeltaTime * direction);
    }

    private void Rotate(Vector3 direction)
    {
        if (direction == Vector3.zero)
            return;

        Quaternion toRotation = Quaternion.LookRotation(direction, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void ActivateImpulse(float value)
    {
        _impulseMultiplier = value;
        _impulseActive = true;
    }

    private void ResetImpulseInternal()
    {
        _impulseMultiplier = DefaultImpulseMultiplier;
        _impulseActive = false;
    }
}
