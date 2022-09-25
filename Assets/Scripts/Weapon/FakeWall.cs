using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWall : MonoBehaviour
{
    [SerializeField]
    private int health = 10;     // 生命值

    [SerializeField]
    private Collider col;

    [SerializeField]
    public float distanceToPlayer = 0.5f;

    private Transform player;

    public event Action<FakeWall> onFakeWallLeavePlayer;
    public event Action<FakeWall> onDestroy;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (col.enabled) return;
        float distance = Vector3.Distance(gameObject.transform.position, player.position);
        if (distance > distanceToPlayer)
        {
            col.enabled = true;
            onFakeWallLeavePlayer?.Invoke(this);
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("墙当前的生命值:" + health);
        if (health <= 0)
        {
            health = 0;

            onDestroy?.Invoke(this);

            gameObject.SetActive(false);

            NavMeshManager.Instance.BakeNavMesh();

            Destroy(gameObject);
        }
    }
}
