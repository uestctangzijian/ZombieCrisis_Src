using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]
    public int maxHealth = 5;

    private int health;

    public event Action<int> onDealth = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (health == 0) return;

        health -= damage;

        if (health <= 0)
        {
            health = 0;
            onDealth?.Invoke(health);
        }
    }
}
