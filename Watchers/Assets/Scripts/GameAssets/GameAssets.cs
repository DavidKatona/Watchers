using System.Runtime.CompilerServices;
using UnityEngine;

namespace Assets.Scripts.GameAssets
{
    public class GameAssets : MonoBehaviour
    {
        private static GameAssets _i;

        public static GameAssets i
        {
            get
            {
                if (_i == null) _i = Instantiate(Resources.Load<GameAssets>("GameAssets"));
                return _i;
            }
        }

        public Transform prefabDamagePopup;
        public Transform prefabTutorialPopup;
        public Transform prefabHitEffect;
        public Transform prefabDeathEffect;
        public Transform prefabPlayerBeingHitEffect;
        public Transform prefabAbyssBolt;
        public Transform prefabAbyssBoltExplosionEffect;
        public Transform prefabAbyssBoltCastEffect;
    }
}
