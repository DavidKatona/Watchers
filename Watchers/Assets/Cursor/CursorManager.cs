using UnityEngine;

namespace Assets.Cursor
{
    public static class CursorManager
    {
        public static void LockCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        public static void UnlockCursor()
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }
}