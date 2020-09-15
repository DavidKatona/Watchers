using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SceneManagement
{
    public class AutoSceneSwitcher : MonoBehaviour
    {
        [SerializeField] private Scene _sceneToLoad;
        [SerializeField] private float _delayTime;

        public void Setup(Scene sceneToLoad, float delay)
        {
            _sceneToLoad = sceneToLoad;
            _delayTime = delay;
        }

        private void Start()
        {
            StartCoroutine(DelaySceneLoad(_sceneToLoad, _delayTime));
        }

        private IEnumerator DelaySceneLoad(Scene scene, float delay)
        {
            yield return new WaitForSeconds(delay);
            Loader.Load(scene, false);
        }
    }
}
