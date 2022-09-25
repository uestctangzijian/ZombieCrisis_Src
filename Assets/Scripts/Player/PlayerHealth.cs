using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;
    public bool isAlive = true;

    private PlayerMove playerMove;
    private Animator animator;
    private Collider col;

    public BloodBar bloodBar;

    [SerializeField]
    private GameObject bloodPrefab;

    public event Action<int> onDealth = delegate { };

    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        animator = GetComponent<Animator>();
        col = GetComponent<Collider>();

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int damage, Vector3 impact)
    {
        health -= damage;

        spawnBlood();
        bloodBar.updateValue(health, maxHealth);
        playerMove.knockback(impact);

        if (health <= 0)
        {
            health = 0;
            Die();
        }
    }

    public void Heal(int hp)
    {
        health += hp;
        if (health >= maxHealth)
            health = maxHealth;
        bloodBar.updateValue(health, maxHealth);
    }

    private void Die()
    {
        if (!isAlive) return;
        isAlive = false;
        col.enabled = false;
        animator.SetTrigger("die");
        bloodBar.hide();
        AudioManager.Instance.Play("PlayerDead");
        onDealth?.Invoke(health);
        //Destroy(gameObject, 3f);
    }

    private void spawnBlood()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = UnityEngine.Random.Range(0.01f, 0.05f);
        float zRotation = UnityEngine.Random.Range(0f, 180f);
        Transform bloodTrans = Instantiate(bloodPrefab, spawnPos, Quaternion.Euler(90f, 0, zRotation)).transform;
        bloodTrans.parent = GameObject.Find("Bloods").transform;
        Vector3 randomizedScale = bloodTrans.transform.localScale;
        randomizedScale.x *= UnityEngine.Random.Range(1.0f, 1.2f);
        randomizedScale.y *= UnityEngine.Random.Range(1.0f, 1.2f);
        bloodTrans.transform.localScale = randomizedScale;

        Destroy(bloodTrans.gameObject, 60f);
    }

}
