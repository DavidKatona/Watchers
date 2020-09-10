using UnityEngine;

namespace Assets.Cursor
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
