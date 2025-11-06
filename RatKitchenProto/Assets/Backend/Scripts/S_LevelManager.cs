using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class S_LevelManager : MonoBehaviour
{
    public static S_LevelManager Instance;

    [Header("Canvas Refs")]
    [SerializeField] private GameObject MainMenuCanvas;
    [SerializeField] private GameObject LoadingScreenCanvas;
    [Header("Loading Screen Refs")]
    [SerializeField] private Slider LoadingScreenBarR;
    [SerializeField] private Slider LoadingScreenBarL;
    [SerializeField] private TMP_Text LoadingText;

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // Singleton
        }

        #endregion
    }

    public void LoadLevel(string LevelName)
    {
        if (LoadingScreenCanvas != null)
        {
            LoadingScreenCanvas.SetActive(true);

            if (MainMenuCanvas != null)
            {
                MainMenuCanvas.SetActive(false);
            }
        }
        StartCoroutine(LoadLevelAsync(LevelName));
    }

    private IEnumerator LoadLevelAsync(string LevelName)
    {
        AsyncOperation LoadOperation = SceneManager.LoadSceneAsync(LevelName);
        LoadOperation.allowSceneActivation = false;

        while (!LoadOperation.isDone)
        {
            float Progress = Mathf.Clamp01(LoadOperation.progress / 0.9f);
            LoadingScreenBarR.value = Progress;
            LoadingScreenBarL.value = Progress;
            LoadingText.text = "Loading... " + (int)(Progress * 100f) + "%";
            if (LoadOperation.progress >= 0.9f)
            {
                LoadingText.text = "Finishing...";
                LoadingScreenBarR.value = 1f;
                LoadingScreenBarL.value = 1f;

                yield return new WaitForSeconds(0.25f);
                LoadOperation.allowSceneActivation = true;
                yield return new WaitForSeconds(0.5f);
                LoadingScreenCanvas.SetActive(false);
            }
            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


// Path: Assets/Backend/Scripts/S_GameManager.cs
// Sort Layer for LoadingScreen to always be above the Scenes its loading, seems obvious but whatever.