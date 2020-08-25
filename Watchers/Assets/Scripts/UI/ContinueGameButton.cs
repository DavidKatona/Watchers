using UnityEngine;
using UnityEngine.UI;

public class ContinueGameButton : MonoBehaviour
{
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("hasSaved"))
        {
            _button.interactable = true;
        }
        else
        {
            _button.interactable = false;
        }
    }
}
