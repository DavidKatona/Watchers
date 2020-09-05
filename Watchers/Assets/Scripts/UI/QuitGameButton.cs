using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class QuitGameButton : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}
