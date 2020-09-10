using Assets.Cursor;
using Assets.Scripts.PersistentDataManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenuUI;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private SaveManager _saveManager;
    public static bool GameIsPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
                _audioSource.clip = _audioClip;
                _audioSource.Play();
            }
        }
    }

    public void Resume()
    {
        _pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    private void Pause()
    {
        _pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadMenu()
    {
        _saveManager.SavePlayerData();
        Loader.Load(Scene.MainMenu, true);
        Resume();
    }

    public void CloseGame()
    {
        _saveManager.SavePlayerData();
        Application.Quit();
    }
}
