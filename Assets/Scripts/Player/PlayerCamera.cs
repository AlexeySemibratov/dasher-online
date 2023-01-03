using Cinemachine;
using Mirror;
using UnityEngine;

public class PlayerCamera : NetworkBehaviour
{
    private Camera _mainCamera;
    private CinemachineFreeLook _freeLookCamera;

    private void Awake()
    {
        _mainCamera = Camera.main;

        // TODO Think how to avoid Find object call.
        _freeLookCamera = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>();
    }

    public override void OnStartLocalPlayer()
    {
        if (_mainCamera != null)
        {
            _freeLookCamera.Follow = transform;
            _freeLookCamera.LookAt = transform;
        }
    }

    public Transform GetMainCameraTransform()
    {
        return _mainCamera.transform;
    }
}
