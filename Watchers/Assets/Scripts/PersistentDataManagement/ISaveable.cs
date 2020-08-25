using UnityEngine;

namespace Assets.Scripts.PersistentDataManagement
{
    public interface ISaveable
    {
        Vector2 GetPoisition();
        void SetPosition(Vector2 position);
        float GetPositionX();
        float GetPositionY();
    }
}
