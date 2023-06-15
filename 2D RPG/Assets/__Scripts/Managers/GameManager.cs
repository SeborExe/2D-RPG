using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : SingletonMonobehaviour<GameManager>, ISaveManager
{
    [SerializeField] private List<CheckPoint> checkPoints = new List<CheckPoint>();

    [SerializeField] private GameObject corpsPrefab;
    [SerializeField] private float corpsXPosition;
    [SerializeField] private float corpsYPosition;
    public int LostCurrency { get; set; }

    private Transform player;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        player = PlayerManager.Instance.player.transform;
    }

    public void AddCheckpointToList(CheckPoint checkPoint)
    {
        checkPoints.Add(checkPoint);
    }

    public async void LoadData(GameData data)
    {
        foreach (var checkPoint in from KeyValuePair<string, bool> pair in data.checkpoints
                                   from CheckPoint checkPoint in checkPoints
                                   where checkPoint.checkpointID == pair.Key && pair.Value == true
                                   select checkPoint)
        {
            checkPoint.ActivateCheckpoint();
        }

        string closestCheckpointID = data.closestCheckPointID;

        await PlacePlayerAtClosestCheckpoint(data);
        await LoadCorps(data);
    }

    public void SaveData(ref GameData data)
    {
        data.lostCurrencyAmount = LostCurrency;
        data.lostCurrencyX = player.position.x;
        data.lostCurrencyY = player.position.y;

        if (FindClosestCheckpoint() != null)
            data.closestCheckPointID = FindClosestCheckpoint().checkpointID;

        data.checkpoints.Clear();

        foreach (CheckPoint checkPoint in checkPoints)
        {
            data.checkpoints.Add(checkPoint.checkpointID, checkPoint.activated);
        }
    }

    private CheckPoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        CheckPoint closestCheckpoint = null;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.position, checkPoint.transform.position);
            if (distanceToCheckpoint < closestDistance && checkPoint.activated)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkPoint;
            }
        }

        return closestCheckpoint;
    }

    private async Task PlacePlayerAtClosestCheckpoint(GameData data)
    {
        await Task.Delay(100);

        if (data.closestCheckPointID == null) return;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (data.closestCheckPointID == checkPoint.checkpointID)
            {
                player.position = checkPoint.transform.position;
            }
        }
    }

    private async Task LoadCorps(GameData data)
    {
        await Task.Delay(100);

        LostCurrency = data.lostCurrencyAmount;
        corpsXPosition = data.lostCurrencyX;
        corpsYPosition = data.lostCurrencyY;

        if (LostCurrency > 0)
        {
            GameObject newCorps = Instantiate(corpsPrefab, new Vector3(corpsXPosition, corpsYPosition), Quaternion.identity);
            newCorps.GetComponent<Corps>().Currency = LostCurrency;
        }

        LostCurrency = 0;
    }

    public void RestartGame()
    {
        SaveManager.Instance.SaveGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PauseGame(bool pause)
    {
        if (pause)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }
}
