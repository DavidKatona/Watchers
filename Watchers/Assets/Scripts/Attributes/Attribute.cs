using System;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
    [System.Serializable]
    public class Attributes
    {
        public event EventHandler OnAttributeChanged;

        public static int ATTR_MIN = 0;
        public static int ATTR_MAX = 99;

        public enum AttributeType
        {
            Vigor,
            Spirit,
            Strength,
            Intelligence,
            Resilience,
            Vitality,
            Focus
        }

        private SingleAttribute _vigor;
        private SingleAttribute _spirit;
        private SingleAttribute _strength;
        private SingleAttribute _intelligence;
        private SingleAttribute _resilience;
        private SingleAttribute _vitality;
        private SingleAttribute _focus;

        public Attributes(int vigorAmount, int spiritAmount, int strengthAmount, int intelligenceAmount, int resilienceAmount, int vitalityAmount, int focusAmount)
        {
            _vigor = new SingleAttribute(vigorAmount);
            _spirit = new SingleAttribute(spiritAmount);
            _strength = new SingleAttribute(strengthAmount);
            _intelligence = new SingleAttribute(intelligenceAmount);
            _resilience = new SingleAttribute(resilienceAmount);
            _vitality = new SingleAttribute(vitalityAmount);
            _focus = new SingleAttribute(focusAmount);
        }

        private SingleAttribute GetSingleAttribute(AttributeType attributeType)
        {
            switch (attributeType)
            {
                default:
                case AttributeType.Vigor:
                    return _vigor;
                case AttributeType.Spirit:
                    return _spirit;
                case AttributeType.Strength:
                    return _strength;
                case AttributeType.Intelligence:
                    return _intelligence;
                case AttributeType.Resilience:
                    return _resilience;
                case AttributeType.Vitality:
                    return _vitality;
                case AttributeType.Focus:
                    return _focus;
            }
        }

        public void SetAttribute(AttributeType attributeType, int attributeAmount)
        {
            GetSingleAttribute(attributeType).SetAttribute(attributeAmount);
            OnAttributeChanged?.Invoke(this, EventArgs.Empty);
        }

        public void IncreaseAttribute(AttributeType attributeType)
        {
            SetAttribute(attributeType, GetAttributeAmount(attributeType) + 1);
        }

        public void DecreaseAttribute(AttributeType attributeType)
        {
            SetAttribute(attributeType, GetAttributeAmount(attributeType) - 1);
        }

        public int GetAttributeAmount(AttributeType attributeType)
        {
            return GetSingleAttribute(attributeType).GetAttributeAmount();
        }

        public float GetAttributeNormalized(AttributeType attributeType)
        {
            return GetSingleAttribute(attributeType).GetAttributeNormalized();
        }

        // Represents a single attribute of any type.
        private class SingleAttribute
        {
            private int _attribute;

            public SingleAttribute(int attributeAmount)
            {
                SetAttribute(attributeAmount);
            }

            public void SetAttribute(int attributeAmount)
            {
                _attribute = Mathf.Clamp(attributeAmount, ATTR_MIN, ATTR_MAX);
            }

            public int GetAttributeAmount()
            {
                return _attribute;
            }

            public float GetAttributeNormalized()
            {
                return (float)_attribute / ATTR_MAX;
            }
        }
    }
}