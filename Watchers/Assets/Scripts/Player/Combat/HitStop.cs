using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    private static HitStop _instance;
    public static HitStop Instance { get { return _instance; } }
    private bool _isWaiting;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Stop(float duration)
    {
        if (_isWaiting)
            return;

        Time.timeScale = 0.0f;
        StartCoroutine(Wait(duration));
    }

    private IEnumerator Wait(float duration)
    {
        _isWaiting = true;
        yield return new WaitForSecondsRealtime(duration);

        // We set the timscale back to it's default value only if the game is not paused by a menu.
        if (!PauseMenu.GameIsPaused)
        {
            Time.timeScale = 1.0f;
        }

        _isWaiting = false;
    }
}
