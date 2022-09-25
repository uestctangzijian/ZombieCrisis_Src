using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnemyWave {
    [SerializeField]
    private EnemyType enemyType;
    [SerializeField]
    private int amount;
    [SerializeField]
    private float timeBetweenSpawn;

    [HideInInspector]
    public int Count = 0;
    public EnemyType EnemyType
    {
        get
        {
            return enemyType;
        }
        set
        {
            enemyType = value;
        }
    }

    public int Amount
    {
        get
        {
            return amount;
        }
        set
        {
            amount = value;
        }
    }

    public float TimeBetweenSpawn
    {
        get
        {
            return timeBetweenSpawn;
        }
        set
        {
            timeBetweenSpawn = value;
        }
    }

    public bool WaveFinished => Count == amount;
}
