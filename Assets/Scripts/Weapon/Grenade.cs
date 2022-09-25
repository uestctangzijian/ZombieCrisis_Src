using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IExplosive
{
    public static bool clusterExplodeEnable = false; // 爆炸后分裂出四个手榴弹
    public static int explosionNums = 1;            // 爆炸次数（可升级）
    public static float explosionRadius = 2f;       // 爆炸半径（可升级）

    private bool isExploded = false;

    [SerializeField]
    private MeshRenderer mRendenerer;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private GameObject explosionEffect;

    [SerializeField]
    private float explodeDalay = 3f;    // 爆炸延时
    [SerializeField]
    private Transform[] explosionTrans; // 爆炸位置
    [SerializeField]
    private float explosionInterval = 0.1f; // 爆炸间隔

    [SerializeField]
    private float impact = 5f;

    // cluster explode
    [SerializeField]
    private GameObject subGrenade;
    [SerializeField]
    private Transform[] subGrenadeTrans;
    [SerializeField]
    private float splitForce = 2f;

    private Rigidbody rb;
    private int damage;
    private Vector3 speed;


    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    public Vector3 Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        rb.velocity = speed;
        StartCoroutine(explodeAfterDalay(explodeDalay));
    }

    private IEnumerator explodeAfterDalay(float time = 3f)
    {
        if (time == 0.0f) yield return null;
        else yield return new WaitForSeconds(time);

        Explode();
    }

    public void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        mRendenerer.enabled = false;
        col.enabled = false;

        // 多爆
        if (explosionNums == 1)
        {
            explode(explosionTrans[0]);
        }
        else
        {
            StartCoroutine(ExplodeInOrder(explosionInterval));
        }

        // 分裂爆
        if (clusterExplodeEnable)
        {
            clusterExplode();
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
        GameObject explosionEffect = trans.Find("SmallExplosion").gameObject;
        explosionEffect.gameObject.SetActive(true);

        Collider[] hitColliders = Physics.OverlapSphere(trans.position, explosionRadius);
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

    private void clusterExplode()
    {
        for (int i = 0; i < 4; i++)
        {
            GameObject go = Instantiate(subGrenade) as GameObject;
            go.transform.position = subGrenadeTrans[i].position;

            Rigidbody rb = go.GetComponent<Rigidbody>();
            Vector3 direction = Vector3.Normalize(go.transform.position - transform.position);
            rb.AddForce(direction * splitForce, ForceMode.Impulse);

            SubBomb sb = go.GetComponent<SubBomb>();
            sb.Damage = damage / 4;
        }
    }
}
