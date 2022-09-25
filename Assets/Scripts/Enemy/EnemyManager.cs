using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private List<GameObject> enemies;

    public int maxAmountOfZombies;

    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<GameObject>();
    }

    public void AddEnemy(GameObject enemy)
    {
        enemies.Add(enemy);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public int CurrentEnemiesCount
    {
        get
        {
            return enemies.Count;
        }
    }

    public bool canSpawnNewEnemy
    {
        get
        {
            return enemies.Count < maxAmountOfZombies;
        }
    }
}
