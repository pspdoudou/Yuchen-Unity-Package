using System;
using UnityEngine;

public class SetCursorMode : MonoBehaviour
{
    [SerializeField] private CursorLockMode cursorLockMode = CursorLockMode.Locked;
    [SerializeField] private bool cursorVisible = false;
    
    
    private void Start()
    {
        Set();
    }

    public void Set()
    {
        Cursor.visible = cursorVisible;
        Cursor.lockState = cursorLockMode;
    }

    public void SetConfine()
    {
        cursorLockMode = CursorLockMode.Confined;
        Cursor.lockState = cursorLockMode;
    }

    public void SetNone()
    {
        cursorLockMode = CursorLockMode.None;
        Cursor.lockState = cursorLockMode;
    }

    public void SetLock()
    {
        cursorLockMode = CursorLockMode.Locked;
        Cursor.lockState = cursorLockMode;
    }

    public void SetVisible(bool visible)
    {
        cursorVisible = visible;
        Cursor.visible = visible;
    }
}
