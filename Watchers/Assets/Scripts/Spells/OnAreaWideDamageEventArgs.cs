using System;
using UnityEngine;

namespace Assets.Scripts.Spells
{
    public class OnAreaWideDamageEventArgs : EventArgs
    {
        private readonly Collider2D[] _collidedObjects;
        private readonly float _damageModifier;

        public Collider2D[] GetCollidedObjects()
        {
            return _collidedObjects;
        }

        public float GetDamageModifier()
        {
            return _damageModifier;
        }

        public OnAreaWideDamageEventArgs(Collider2D[] collidedObjectsArray, float damageModifier)
        {
            _collidedObjects = collidedObjectsArray;
            _damageModifier = damageModifier;
        }
    }
}
