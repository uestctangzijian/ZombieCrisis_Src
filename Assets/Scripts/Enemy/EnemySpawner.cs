using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject zombiePrefab;
    [SerializeField]
    private GameObject bossPrefab;
    [SerializeField]
    public List<EnemyWave> enmeyWaves;

    [SerializeField]
    private float initialSpawnDelay;

    private int currentWaveIndex = 0;

    [SerializeField]
    private EnemyManager enemyManager;

    [SerializeField]
    private PickUpGenerator pickupGenerator;

    private Transform player;
   
    private float countdown;

    public bool hasFinishedSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        countdown = initialSpawnDelay;
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasFinishedSpawning) return;

        if (enmeyWaves[currentWaveIndex].WaveFinished)
        {
            if (currentWaveIndex < enmeyWaves.Count - 1)
            {
                currentWaveIndex++;
                countdown = enmeyWaves[currentWaveIndex].TimeBetweenSpawn;
            } else
            {
                hasFinishedSpawning = true;
                return;
            }
        }

        countdown -= Time.deltaTime;
        EnemyWave wave = enmeyWaves[currentWaveIndex];
        if (countdown <= 0 && !wave.WaveFinished && enemyManager.canSpawnNewEnemy)
        {
            GameObject go = Instantiate(GetEnemyPrefabByType(wave.EnemyType)) as GameObject;
            //go.transform.position = transform.position;
            go.transform.parent = transform;
            go.transform.rotation = transform.localRotation;

            EnemyController enemy = go.GetComponent<EnemyController>();
            // 用warp设置初始位置，可以避免瞬移问题
            // https://answers.unity.com/questions/1830969/navmeshagentwarp-meaning.html
            enemy.agent.Warp(transform.position);
            enemy.enemyManager = enemyManager;
            enemy.pickupGenerator = pickupGenerator;
            enemy.Target = player;
            enemy.type = wave.EnemyType;
            wave.Count++;
            countdown = wave.TimeBetweenSpawn;

            enemyManager.AddEnemy(enemy.gameObject);
        }
    }

    private GameObject GetEnemyPrefabByType(EnemyType type)
    {
        if (type == EnemyType.Zombie)
            return zombiePrefab;
        else
            return bossPrefab;
    }

    public void Refresh()
    {
        countdown = initialSpawnDelay;
        hasFinishedSpawning = false;
        currentWaveIndex = 0;
    }
}
