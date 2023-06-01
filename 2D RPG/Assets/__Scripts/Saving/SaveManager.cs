using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SaveManager : SingletonMonobehaviour<SaveManager>
{
    private readonly string fileName = "MainSave";
    [SerializeField] private bool encryptData = true;

    private GameData gameData;
    private FileDataHandler fileDataHandler;

    private List<ISaveManager> saveManagers = new List<ISaveManager>();

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        saveManagers = FindAllSaveManagers();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);
    }

    [ContextMenu("Delete Save File")]
    private void DeleteSaveData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        fileDataHandler.Delete();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSaveManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>().OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
}
