using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameUI : MonoBehaviour, ISaveManager
{
    [field: SerializeField] public ItemTooltipUI itemTooltipUI { get; private set; }
    [field: SerializeField] public StatTooltipUI statTooltipUI { get; private set; }
    [field: SerializeField] public SkillTooltipUI skillTooltipUI { get; private set; }
    [field: SerializeField] public CraftWindowUI craftWindowUI { get; private set; }

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject skillUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;

    [Space, SerializeField] private FadeScreenUI fadeScreenUI;
    [SerializeField] private GameObject youDiedGameObject;
    [SerializeField] private Button BackToMainMenuButton;

    [SerializeField] private VolumeSliderUI[] volumeSettings;

    private void Awake()
    {
        SwitchTo(skillUI); //Assign events on skill tree slot 
    }

    private void Start()
    {
        SwitchTo(inGameUI);

        PlayerManager.Instance.OnPlayerDie += PlayerManager_OnPlayerDie;
    }

    private void OnDisable()
    {
        PlayerManager.Instance.OnPlayerDie -= PlayerManager_OnPlayerDie;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
            SwitchWithKeyTo(characterUI);

        if (Input.GetKeyDown(KeyCode.O))
            SwitchWithKeyTo(skillUI);

        if (Input.GetKeyDown(KeyCode.B))
            SwitchWithKeyTo(craftUI);

        if (Input.GetKeyDown(KeyCode.Escape))
            SwitchWithKeyTo(optionsUI);
    }

    public void SwitchTo(GameObject menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            bool isFadeScreen = transform.GetChild(i).GetComponent<FadeScreenUI>() != null;

            if (!isFadeScreen)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        if (menu != null)
        {
            menu.SetActive(true);

            if (AudioManager.Instance != null)
                AudioManager.Instance.PlaySFX(7, null);
        }

        if (GameManager.Instance != null)
        {
            if (menu == inGameUI)
                GameManager.Instance.PauseGame(false);
            else
                GameManager.Instance.PauseGame(true);
        }
    }

    public void SwitchWithKeyTo(GameObject menu)
    {
        if (menu != null && menu.activeSelf)
        {
            menu.SetActive(false);
            CheckForInGameUI();
            return;
        }

        SwitchTo(menu);
    }

    private void CheckForInGameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf && transform.GetChild(i).GetComponent<FadeScreenUI>() == null)
                return;
        }

        SwitchTo(inGameUI);
    }

    private void PlayerManager_OnPlayerDie()
    {
        fadeScreenUI.FadeOut();
        youDiedGameObject.SetActive(true);
        BackToMainMenuButton.gameObject.SetActive(true);
        BackToMainMenuButton.onClick.AddListener(() =>
        {
            SaveManager.Instance.DeleteSaveData();
            SceneManager.LoadScene(0);
        });
    }

    public void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, float> pair in data.volumeSettings)
        {
            foreach (VolumeSliderUI item in volumeSettings)
            {
                if (item.parameter == pair.Key)
                    item.LoadSlider(pair.Value);
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.volumeSettings.Clear();

        foreach (VolumeSliderUI item in volumeSettings)
        {
            data.volumeSettings.Add(item.parameter, item.slider.value);
        }
    }
}
