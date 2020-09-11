using UnityEngine;

namespace Assets.Scripts.Cursor
{
    public class CursorInitializer : MonoBehaviour
    {
        private void OnEnable()
        {
            CursorManager.UnlockCursor();
        }

        private void OnDisable()
        {
            CursorManager.LockCursor();
        }
    }
}