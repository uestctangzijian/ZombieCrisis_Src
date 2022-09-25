using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Fireball, IExplosive
{
    public static int explosionNums = 1;    // ±¬Õ¨´ÎÊý£¨¿ÉÉý¼¶£©
    public static float explosionRadius = 2; // ±¬Õ¨·¶Î§ £¨¿ÉÉý¼¶£©

    [SerializeField]
    private Transform[] explosionTrans;     // ±¬Õ¨Î»ÖÃ
    [SerializeField]
    private float explosionInterval = 0.1f; // ±¬Õ¨¼ä¸ô

    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private Collider col;

    [SerializeField]
    private LayerMask explosionInducingLayers;

    [SerializeField]
    private LayerMask explosionAffectedLayers;

    private bool isExploded = false;

    void OnCollisionEnter(Collision collision)
    {
        GameObject other = collision.gameObject;

        if (explosionInducingLayers == (explosionInducingLayers | (1 << other.layer))) {
            Explode();
        }
    }

    public void Explode()
    {
        if (isExploded) return;

        isExploded = true;
        col.enabled = false;

        if (explosionNums == 1)
        {
            explode(explosionTrans[0]);
        }
        else
        {
            StartCoroutine(ExplodeInOrder(explosionInterval));
        }

        Destroy(gameObject);
    }

    private IEnumerator ExplodeInOrder(float interval)
    {
        for (int i = 0; i < explosionTrans.Length; i++)
        {
            explode(explosionTrans[i]);

            yield return new WaitForSeconds(interval);
        }
    }

    private void explode(Transform transform)
    {
        Destroy(Instantiate(explosionEffect, transform.position, Quaternion.identity), 3f);
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius, explosionAffectedLayers);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Enemy"))
            {
                EnemyController enemy = hitColliders[i].GetComponent<EnemyController>();
                enemy.TakeDamage(damage, transform.forward * impact);
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
