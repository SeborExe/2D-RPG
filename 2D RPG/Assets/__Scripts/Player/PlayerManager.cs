using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonobehaviour<PlayerManager>, ISaveManager
{
    public event Action OnCurrencyChanged;
    public event Action OnPlayerDie;

    public Player player;

    [field: SerializeField]public int Currency { get; private set; }

    protected override void Awake()
    {
        base.Awake();
    }

    public bool HaveEnoughMoney(int priece)
    {
        if (priece > Currency)
            return false;

        Currency -= priece;
        OnCurrencyChanged?.Invoke();
        return true;
    }

    public void LoadData(GameData data)
    {
        Currency = data.currency;
    }

    public void SaveData(ref GameData data)
    {
        data.currency = Currency;
    }
    
    public void InvokeOnPlayerDie() => OnPlayerDie?.Invoke();
}
