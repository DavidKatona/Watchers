using System;
using UnityEngine;

namespace Assets.Scripts.Attributes
{
    [System.Serializable]
    public class Attributes
    {
        public event EventHandler OnAttributeChanged;
        public event EventHandler OnSoulsChanged;

        private const int baseLevel = 1;
        private const int baseSoulsRequired = 50;
        public static int ATTR_MIN = 11;
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

        private int _watcherLevel = baseLevel;
        public int GetLevel()
        {
            return _watcherLevel;
        }

        private void SetLevel(int amount)
        {
            _watcherLevel = baseLevel + amount;
        }

        // This should probably be part of an inventory class or something else that keeps track of resources.
        private int _souls;
        public int GetSouls()
        {
            return _souls;
        }

        public void SetSouls(int amount)
        {
            _souls = amount;
            OnSoulsChanged?.Invoke(this, EventArgs.Empty);
        }

        private int _soulsRequired = baseSoulsRequired;
        public int GetSoulsRequired()
        {
            return _soulsRequired;
        }

        private void SetSoulsRequired(int amount)
        {
            _soulsRequired = baseSoulsRequired + amount;
        }

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

        /// <summary>
        /// Gets a single attribute by an attribute type.
        /// </summary>
        /// <param name="attributeType">The type of attribute which will be returned.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Sets a given attribute type to the specified value.
        /// </summary>
        /// <param name="attributeType">The type of attribute which will be set.</param>
        /// <param name="attributeAmount">The value the passed attribute will be set to.</param>
        public void SetAttribute(AttributeType attributeType, int attributeAmount)
        {
            GetSingleAttribute(attributeType).SetAttribute(attributeAmount);
            SetLevel(GetTotalAmountOfAttributes());
            SetSoulsRequired(CalculateSoulsRequired());
            OnAttributeChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Increments a given attribute by one.
        /// </summary>
        /// <param name="attributeType">The type of attribute which will be incremented.</param>
        public void IncreaseAttribute(AttributeType attributeType)
        {
            if (_souls >= _soulsRequired)
            {
                _souls -= _soulsRequired;
                SetAttribute(attributeType, GetAttributeAmount(attributeType) + 1);
            }
        }

        /// <summary>
        /// Decrements a given attribute by one.
        /// </summary>
        /// <param name="attributeType">The type of attribute which will be decremented.</param>
        public void DecreaseAttribute(AttributeType attributeType)
        {
            SetAttribute(attributeType, GetAttributeAmount(attributeType) - 1);
        }

        /// <summary>
        /// Returns the value of a given attribute.
        /// </summary>
        /// <param name="attributeType">The type of attribute whose value will be returned.</param>
        /// <returns></returns>
        public int GetAttributeAmount(AttributeType attributeType)
        {
            return GetSingleAttribute(attributeType).GetAttributeAmount();
        }

        /// <summary>
        /// Returns the normalized value of a given attribute (attribute/maximum value of attribute).
        /// </summary>
        /// <param name="attributeType">The type of attribute whose normalized value will be returned.</param>
        /// <returns></returns>
        public float GetAttributeNormalized(AttributeType attributeType)
        {
            return GetSingleAttribute(attributeType).GetAttributeNormalized();
        }

        /// <summary>
        /// Gets the total amount of points invested into attributes.
        /// </summary>
        /// <returns></returns>
        private int GetTotalAmountOfAttributes()
        {
            var totalInvestmentInAttributes = 0;
            var attributeCollection = Enum.GetValues(typeof(AttributeType));

            foreach (var attribute in attributeCollection)
            {
                // We calculate the amount of points we have invested onto attributes by getting their current amount and subtracting the attribute minimum.
                totalInvestmentInAttributes += GetAttributeAmount((AttributeType)attribute) - ATTR_MIN;
            }

            return totalInvestmentInAttributes;
        }

        /// <summary>
        /// Calculates the amount of souls required for the next level.
        /// </summary>
        /// <returns></returns>
        private int CalculateSoulsRequired()
        {
            var soulsMultiplier = 100;
            return (int)(Mathf.Pow(GetTotalAmountOfAttributes(), 2) * soulsMultiplier);
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