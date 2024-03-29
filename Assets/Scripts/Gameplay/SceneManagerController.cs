using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    [SerializeField] private float optionsUnloadDelay;

    public void LoadMainGame()
    {
        var playButtonAudioSource = GetComponent<AudioSource>();
        var playButtonAudioClipLength = playButtonAudioSource.clip.length;
        StartCoroutine(LoadPlatformSection(playButtonAudioClipLength));
        
    }

    public void LoadTestPlatformer()
    {
        SceneManager.LoadScene("PlatformSection2");
    }

    public void LoadTestBoss()
    {
        SceneManager.LoadScene("MageBoss");
    }

    private IEnumerator LoadPlatformSection(float playButtonAudioClipLength)
    {
        yield return new WaitForSeconds(playButtonAudioClipLength / 3f);
        SceneManager.LoadScene("PlatformSection");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options", LoadSceneMode.Additive);
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void UnloadOptions()
    {
        GameObject.FindGameObjectWithTag("MainMenuCanvas").SetActive(true);
        StartCoroutine(DelayUnloadOptions());
    }

    private IEnumerator DelayUnloadOptions()
    {
        yield return new WaitForSeconds(optionsUnloadDelay);
        SceneManager.UnloadSceneAsync("Options");
    }
}