using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public static HitStop Instance { get; private set; }
    private bool _isWaiting;

    private void Awake()
    {
        Instance = this;
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
        Time.timeScale = 1.0f;
        _isWaiting = false;
    }
}
