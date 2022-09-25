using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour, IExplosive
{
    public static int explosionNums = 1;       // ±¬Õ¨´ÎÊý£¨¿ÉÉý¼¶£©
    public static float explosionRadius = 2f;  // ±¬Õ¨°ë¾¶£¨¿ÉÉý¼¶£©

    [SerializeField]
    public int damage; // ÉËº¦
    [SerializeField]
    private Transform[] explosionTrans; // ±¬Õ¨Î»ÖÃ
    [SerializeField]
    private float explosionInterval = 0.1f; // ±¬Õ¨¼ä¸ô
    [SerializeField]
    private GameObject explosionEffect; // ±¬Õ¨ÌØÐ§
    [SerializeField]
    private LayerMask explosionInducingLayers;
    [SerializeField]
    private LayerMask explosionAffectedLayers;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private MeshRenderer[] mRenderers;
    [SerializeField]
    private float impact = 5f;
    [SerializeField]
    public float distanceToPlayer;

    [SerializeField]
    private bool hasLeavePlayer = false;

    private bool isExploded = false;

    private Transform player;

    public event Action<Barrel> onBarrelLeavePlayer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (col.enabled) return;
        float distance = Vector3.Distance(gameObject.transform.position, player.position);
        if (distance > distanceToPlayer && !hasLeavePlayer)
        {
            col.enabled = true;
            onBarrelLeavePlayer?.Invoke(this);
            hasLeavePlayer = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (explosionInducingLayers == (explosionInducingLayers | (1 << collision.gameObject.layer)))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (isExploded) return;
         isExploded = true;

        foreach(MeshRenderer mr in mRenderers)
        {
            mr.enabled = false;
        }
        col.enabled = false;

        if (explosionNums == 1)
        {
            explode(explosionTrans[0]);
        } else
        {
            StartCoroutine(ExplodeInOrder(explosionInterval));
        }

        Destroy(gameObject, 2.5f);
    }

    private IEnumerator ExplodeInOrder(float interval)
    {
        for (int i = 0; i < explosionTrans.Length; i++)
        {
            explode(explosionTrans[i]);

            yield return new WaitForSeconds(interval);
        }
    }

    private void explode(Transform trans)
    {
        GameObject explosionEffect = trans.Find("BigExplosion").gameObject;
        explosionEffect.gameObject.SetActive(true);

        Collider[] hitColliders = Physics.OverlapSphere(trans.position, explosionRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            EnemyController enemy = hitColliders[i].GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector3 direction = trans.position - enemy.gameObject.transform.position;
                enemy.TakeDamage(damage, direction * impact);
            }
            else
            {
                if (hitColliders[i].gameObject.GetComponent<MonoBehaviour>() is IExplosive explosive)
                {
                    if (explosive != this)
                    {
                        explosive.Explode();
                    }
                }
                if (hitColliders[i].transform.CompareTag("FakeWall"))
                {
                    FakeWall fakewall = hitColliders[i].transform.parent.GetComponent<FakeWall>();
                    if (fakewall != null)
                    {
                        fakewall.TakeDamage(damage);
                    }
                }
            }
        }
        AudioManager.Instance.Play("Explosion");
    }
}
