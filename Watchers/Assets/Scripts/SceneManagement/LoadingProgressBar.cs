using UnityEngine;
using UnityEngine.UI;

public class LoadingProgressBar : MonoBehaviour
{
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (_image.fillAmount < Loader.GetLoadingProgress())
        {
            _image.fillAmount += 0.01f; // Depends on the size of the scene we want to load.
        }

        //_image.fillAmount = Loader.GetLoadingProgress();
    }
}
