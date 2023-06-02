using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private Button newGameButton;
    [SerializeField] private FadeScreenUI fadeScreen;
    private readonly int gameSceneToLoadIndex = 1;
    private const float sceneLoadDelay = 1f;

    private void Start()
    {
        if (!SaveManager.Instance.HasSaveData())
        {
            continueButton.interactable = false;
            continueButton.GetComponentInChildren<TMP_Text>().alpha = 0.5f;
        }
        else
        {
            continueButton.interactable = true;
            continueButton.GetComponentInChildren<TMP_Text>().alpha = 1f;
        }
    }

    private void OnEnable()
    {
        continueButton.onClick.AddListener(() => StartCoroutine(LoadSceneWithFadeEffect(sceneLoadDelay)));
        newGameButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.DeleteSaveData();
            StartCoroutine(LoadSceneWithFadeEffect(sceneLoadDelay));
        });
    }

    private void OnDisable()
    {
        continueButton.onClick.RemoveAllListeners();
        newGameButton.onClick.RemoveAllListeners();
    }

    private IEnumerator LoadSceneWithFadeEffect(float delay)
    {
        fadeScreen.FadeOut();

        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(gameSceneToLoadIndex);
    }
}
