using Assets.Cursor;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    /// <summary>
    /// An empty private class to perform monobehaviour operations on.
    /// </summary>
    private class LoadingMonoBehaviour : MonoBehaviour { }

    private static Action _onLoaderCallback;
    private static AsyncOperation _loadingAsyncOperation;

    public static void Load(Scene scene, bool shouldGoToLoadingScreen)
    {
        if (shouldGoToLoadingScreen)
        {
            // Before we load the loading scene, we assign an anonymous function to our static onLoaderCallback action, which will be called by the LoaderCallback method.
            _onLoaderCallback = () =>
            {
            // We create an empty GameObject which will hold a component that implements MonoBehaviour so we can call StartCoroutine on that object.
            GameObject loadingGameObject = new GameObject("Loading Game Object");
                loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
            };

            SceneManager.LoadScene(Scene.LoadingScene.ToString());
            // We lock the cursor each time a new level is loaded. The loaded level will decide later if it should unlock the cursor or not.
            CursorManager.LockCursor();
        }
        else
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadSceneAsync(scene));
        }
    }

    /// <summary>
    /// A coroutine that asynchronously loads a target scene.
    /// </summary>
    /// <param name="scene">Scene to load.</param>
    /// <returns></returns>
    private static IEnumerator LoadSceneAsync(Scene scene)
    {
        yield return null;
        _loadingAsyncOperation = SceneManager.LoadSceneAsync(scene.ToString());

        while (!_loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (_loadingAsyncOperation != null)
        {
            return _loadingAsyncOperation.progress;
        }
        else
        {
            return 1f;
        }
    }

    /// <summary>
    /// Invokes the static onLoaderCallback action of the static Loader class if it is not null.
    /// </summary>
    public static void LoaderCallback()
    {
        _onLoaderCallback?.Invoke();
    }
}