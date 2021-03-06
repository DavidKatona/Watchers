﻿using UnityEngine;

namespace Assets.Scripts.GameAssets
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _instance;

        public static GameAssets Instance
        {
            get
            {
                if (_instance == null) _instance = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _instance;
            }
        }

        public Transform prefabDamagePopup;
        public Transform prefabTutorialPopup;
        public Transform prefabHitEffect;
        public Transform prefabDeathEffect;
        public Transform prefabPlayerBeingHitEffect;
        public Transform prefabPlayerDeathEffect;
        public Transform prefabAbyssBolt;
        public Transform prefabAbyssBoltExplosionEffect;
        public Transform prefabAbyssBoltCastEffect;
        public Transform prefabCollectibleSoul;
        public Transform prefabCollectibleSoulPicked;
        public Transform prefabCollectibleHealth;
        public Transform prefabCollectibleHealthPicked;
        public Transform prefabCollectibleMana;
        public Transform prefabCollectibleManaPicked;
    }
}
