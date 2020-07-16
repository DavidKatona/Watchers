using UnityEngine;

namespace Assets.Scripts.Attributes
{
    public class AttributeManager : MonoBehaviour
    {
        public Attribute Vigor = new Attribute(10);
        public Attribute Spirit = new Attribute(10);
        public Attribute Strength = new Attribute(10);
        public Attribute Intelligence = new Attribute(10);
        public Attribute Resilience = new Attribute(10);
        public Attribute Vitality = new Attribute(10);
        public Attribute Focus = new Attribute(10);
    }
}
