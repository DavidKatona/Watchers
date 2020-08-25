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

        public float GetHealth()
        {
            return _statManager.CurrentHealth;
        }

        public void SetHealth(float value)
        {
            _statManager.SetCurrentHealth(value);
        }

        public float GetMana()
        {
            return _statManager.CurrentMana;
        }

        public void SetMana(float value)
        {
            _statManager.SetCurrentMana(value);
        }

        public Vector2 GetPoisition()
        {
            return _playerTransform.position;
        }

        public void SetPosition(Vector2 position)
        {
            _playerTransform.position = position;
        }

        public float GetPositionX()
        {
            return _playerTransform.position.x;
        }

        public float GetPositionY()
        {
            return _playerTransform.position.y;
        }

        public int GetSouls()
        {
            return _statManager.GetAttributes().GetSouls();
        }

        public void SetSouls(int value)
        {
            throw new System.NotImplementedException();
        }

        public int GetSoulsRequired()
        {
            return _statManager.GetAttributes().GetSouls();
        }

        public void SetSoulsRequired(int value)
        {
            throw new System.NotImplementedException();
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
