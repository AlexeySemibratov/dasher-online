using Mirror;
using UnityEngine;

public class CursorVisibilityController : NetworkBehaviour
{
    private const int MouseButtonLMB = 0;

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        LockCursor();
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        } 
        else if (Input.GetMouseButton(MouseButtonLMB))
        {
            LockCursor();
        }
    }

    private void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
    }

    private void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked; ;
    }
}
