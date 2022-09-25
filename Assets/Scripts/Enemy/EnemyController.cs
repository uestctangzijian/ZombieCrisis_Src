using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private EnemyTrace trace;
    [SerializeField]
    private EnemyHealth health;
    [SerializeField]
    private EnemyAttack attack;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    public NavMeshAgent agent;

    [SerializeField]
    private GameObject bloodPrefab;

    [SerializeField]
    public int points; // ��һ�ɱ�õ��˻�õķ���

    [SerializeField]
    private float dropChance; // ��Ʒ���伸��

    public PickUpGenerator pickupGenerator;

    public EnemyManager enemyManager;

    private Transform target;

    private bool isAlive;

    public EnemyType type;

    void OnEnable()
    {
        health.onDealth += Die;
    }

    private void OnDisable()
    {
        health.onDealth -= Die;
    }
    void Awake()
    {
        IsAlive = true;
    }

    // �����յ��˺�
    public void TakeDamage(int damage, Vector3 knockbackForce)
    {
        // 1. ��Ѫ
        health.TakeDamage(damage);
        // 2. ����
        trace.Knockback(knockbackForce);
        // 3. Ѫ��
        spawnBlood();
    }

    private void spawnBlood()
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y = Random.Range(0.01f, 0.05f);
        float zRotation = Random.Range(0f, 180f);
        Transform bloodTrans = Instantiate(bloodPrefab, spawnPos, Quaternion.Euler(90f, 0, zRotation)).transform;
        bloodTrans.parent = GameObject.Find("Bloods").transform;
        Vector3 randomizedScale = bloodTrans.transform.localScale;
        randomizedScale.x *= Random.Range(1.0f, 1.2f);
        randomizedScale.y *= Random.Range(1.0f, 1.2f);
        bloodTrans.transform.localScale = randomizedScale;

        Destroy(bloodTrans.gameObject, 60f);
    }

    // ��������
    public void Die(int health)
    {
        if (!IsAlive) return;
        IsAlive = false;
        trace.agent.enabled = false;
        col.enabled = false;

        animator.SetBool("walk", false);
        animator.SetBool("attack", false);
        animator.SetTrigger("die");
        animator.SetBool("dead", true);
        player.KilledEnemy(this);
        enemyManager.RemoveEnemy(gameObject);
        dropItem();
        AudioManager.Instance.Play("EnemyDead");
        Destroy(gameObject, 3.0f);
    }

    public void dropItem()
    {
        if (Random.Range(0f, 1f) <= dropChance)
        {
            PickUp pickup = pickupGenerator.makeItem(transform);
            Destroy(pickup.gameObject, 10f);
        }
    }

    public Transform Target
    {
        get
        {
            return target;
        }
        set
        {
            target = value;
            trace.target = value;
            attack.target = value;
        }
    }

    public bool IsAlive
    {
        get
        {
            return isAlive;
        } 
        set
        {
            isAlive = value;
            trace.isAlive = value;
            attack.isAlive = value;
        }
    }

    public PlayerController player
    {
        get { return target.GetComponent<PlayerController>(); }
    }
}
