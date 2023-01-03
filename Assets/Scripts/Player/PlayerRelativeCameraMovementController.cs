using Mirror;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCamera))]
public class PlayerRelativeCameraMovementController : NetworkBehaviour
{
    [SerializeField]
    private float _movementSpeed = 2.0f;

    [SerializeField]
    private float _rotationSpeed = 360.0f;

    private PlayerCamera _playerCamera;
    private Transform _cameraTransform => _playerCamera.GetMainCameraTransform();

    private CharacterController _characterController;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _playerCamera = GetComponent<PlayerCamera>();
    }


    void Update()
    {
        if (!isLocalPlayer)
            return;

        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        float verticalInput = Input.GetAxisRaw("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        Vector3 movementDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

        if (movementDirection.magnitude == 0)
            return;

        movementDirection = Quaternion.AngleAxis(_cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        RotateTarget(movementDirection);
        _characterController.Move(movementDirection * _movementSpeed * Time.deltaTime);
    }

    private void RotateTarget(Vector3 movementDirection)
    {
        Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, _rotationSpeed * Time.deltaTime);
    }

    private void OnApplicationFocus(bool focus)
    {
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
