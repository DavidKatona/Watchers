using System.Collections;
using UnityEngine;

namespace Assets.Scripts.SceneManagement
{
    public class LoaderCallback : MonoBehaviour
    {
        [SerializeField] bool _delayLoadingTime = true;
        private bool _isFirstUpdate = true;

        // Update is called once per frame
        void Update()
        {
            if (_isFirstUpdate)
            {
                _isFirstUpdate = false;

                if (_delayLoadingTime)
                {
                    StartCoroutine(WaitForLoad());
                }
                else
                {
                    Loader.LoaderCallback();
                }

            }
        }

        private IEnumerator WaitForLoad()
        {
            yield return new WaitForSeconds(2.0f);
            Loader.LoaderCallback();
        }
    }
}
