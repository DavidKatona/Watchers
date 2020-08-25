using UnityEngine;
using UnityEngine.UI;

public class NewGameButton : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(StartNewGame);
    }

    private void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("New game has been started. All previous saves have been deleted.");
    }
}
