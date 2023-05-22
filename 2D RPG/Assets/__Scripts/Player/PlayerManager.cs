using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonobehaviour<PlayerManager>
{
    public Player player;

    public int currency;

    protected override void Awake()
    {
        base.Awake();
    }

    public bool HaveEnoughMoney(int priece)
    {
        if (priece > currency)
            return false;

        currency -= priece;
        return true;
    }
}
