using UnityEngine;
using Assets.Scripts.Player.Skills;
using Assets.Scripts.Attributes;

namespace Assets.Scripts.PersistentDataManagement
{
    public class SaveablePlayer : MonoBehaviour, ISaveablePlayer
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private StatManager _statManager;
        [SerializeField] private SkillManager _skillManager;

        public int GetSouls()
        {
            return _statManager.GetAttributes().GetSouls();
        }

        public void SetSouls(int value)
        {
            _statManager.GetAttributes().SetSouls(value);
        }

        public int GetAttribute(AttributeType attributeType)
        {
            return _statManager.GetAttributes().GetAttributeAmount(attributeType);
        }

        public void SetAttribute(AttributeType attributeType, int value)
        {
            _statManager.GetAttributes().SetAttribute(attributeType, value);
        }
    }
}
