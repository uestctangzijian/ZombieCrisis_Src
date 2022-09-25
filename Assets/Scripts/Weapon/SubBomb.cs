using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubBomb : MonoBehaviour, IExplosive
{
    private bool isExploded = false;

    [SerializeField]
    private MeshRenderer mRendenerer;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private float explosionRadius = 2f; // ±¬Õ¨°ë¾¶
    [SerializeField]
    private float impact = 5f;

    private int damage;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Floor"))
        {
            Explode();
        }
    }

    public void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        mRendenerer.enabled = false;
        col.enabled = false;
        explosionEffect.SetActive(true);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            EnemyController enemy = hitColliders[i].GetComponent<EnemyController>();
            if (enemy != null)
            {
                Vector3 direction = transform.position - enemy.gameObject.transform.position;
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
                if (hitColliders[i].CompareTag("FakeWall"))
                {
                    FakeWall fakewall = hitColliders[i].transform.GetComponent<FakeWall>();
                    if (fakewall != null)
                    {
                        fakewall.TakeDamage(damage);
                    }
                }
            }
        }

        AudioManager.Instance.Play("Explosion");
        Destroy(gameObject, 2.5f);
    }

}
