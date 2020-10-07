using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public enum SceneName
    {
        MainMenu,
        Game,
        ComicStart,
        ComicEnd
    }
    
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider slider;

    private Fader _fader;

    private void Start()
    {
        _fader = FindObjectOfType<Fader>();
    }

    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }


    private IEnumerator LoadAsynchronously(int sceneIndex, bool unloadFirst = false)
    {
        yield return _fader.FadeOut(1.5f);

        loadingScreen.SetActive(true);
        
        if (unloadFirst)
        {
            SceneManager.UnloadSceneAsync(sceneIndex);
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
        
        loadingScreen.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return _fader.FadeIn(1.5f);
    }
}
