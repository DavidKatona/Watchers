using UnityEngine;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private Scene _sceneToLoad;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(LoadScene);
    }

    private void LoadScene()
    {
        Loader.Load(_sceneToLoad);
    }
}
