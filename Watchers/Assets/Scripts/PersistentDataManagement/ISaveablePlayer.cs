namespace Assets.Scripts.PersistentDataManagement
{
    public interface ISaveablePlayer
    {
        int GetSouls();
        void SetSouls(int value);
        int GetAttribute(Attributes.AttributeType attributeType);
        void SetAttribute(Attributes.AttributeType attributeType, int value);
    }
}
