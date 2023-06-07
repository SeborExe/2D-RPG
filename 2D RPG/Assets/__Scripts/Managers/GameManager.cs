using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>, ISaveManager
{
    [SerializeField] private List<CheckPoint> checkPoints = new List<CheckPoint>();

    protected override void Awake()
    {
        base.Awake();
    }

    public void AddCheckpointToList(CheckPoint checkPoint)
    {
        checkPoints.Add(checkPoint);
    }

    public async void LoadData(GameData data)
    {
        foreach (KeyValuePair<string, bool> pair in data.checkpoints)
        {
            foreach (CheckPoint checkPoint in checkPoints)
            {
                if (checkPoint.checkpointID == pair.Key && pair.Value == true)
                {
                    checkPoint.ActivateCheckpoint();
                }
            }
        }

        string closestCheckpointID = data.closestCheckPointID;
        await PlacePlayerAtClosestCheckpoint(closestCheckpointID);
    }

    private async Task PlacePlayerAtClosestCheckpoint(string closestCheckpointID)
    {
        await Task.Delay(100);

        foreach (CheckPoint checkPoint in checkPoints)
        {
            if (closestCheckpointID == checkPoint.checkpointID)
            {
                PlayerManager.Instance.player.transform.position = checkPoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
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
        Player player = PlayerManager.Instance.player;

        foreach (CheckPoint checkPoint in checkPoints)
        {
            float distanceToCheckpoint = Vector2.Distance(player.transform.position, checkPoint.transform.position);
            if (distanceToCheckpoint < closestDistance && checkPoint.activated)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkPoint;
            }
        }

        return closestCheckpoint;
    }
}
