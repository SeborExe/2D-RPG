using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : SingletonMonobehaviour<PlayerManager>
{
    public Player player;

    protected override void Awake()
    {
        base.Awake();
    }
}
