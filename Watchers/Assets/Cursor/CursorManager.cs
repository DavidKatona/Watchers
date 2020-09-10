using UnityEngine;

namespace Assets.Cursor
{
    public static class CursorManager
    {
        private static bool _isCursorLocked;

        public static bool GetCursorState()
        {
            return _isCursorLocked;
        }

        public static void LockCursor()
        {
            _isCursorLocked = true;

            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        public static void UnlockCursor()
        {
            _isCursorLocked = false;

            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
    }
}
