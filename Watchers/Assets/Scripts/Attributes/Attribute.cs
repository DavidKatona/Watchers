using UnityEngine;

namespace Assets.Scripts.Attributes
{
    [System.Serializable]
    public class Attribute
    {
        [SerializeField] private int _value;

        public int GetValue()
        {
            return _value;
        }

        public void Decrement()
        {
            _value--;
        }

        public void Increment()
        {
            _value++;
        }

        public Attribute(int baseValue)
        {
            _value = baseValue;
        }
    }
}
