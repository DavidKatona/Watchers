using UnityEngine;

namespace Assets.Scripts.PersistentDataManagement
{
    public interface ISaveablePlayer : ISaveable
    {
        float GetHealth();
        void SetHealth(float value);
        float GetMana();
        void SetMana(float value);
        int GetSouls();
        void SetSouls(int value);
        int GetAttribute(Attributes.AttributeType attributeType);
        void SetAttribute(Attributes.AttributeType attributeType, int value);
    }
}
