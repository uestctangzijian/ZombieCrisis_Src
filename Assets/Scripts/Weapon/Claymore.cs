using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claymore : MonoBehaviour, IExplosive
{
    public static bool clusterExplodeEnable = false; // ��ը����ѳ��ĸ�����
    public static int explosionNums = 1;            // ��ը��������������
    public static float explosionRadius = 2f;       // ��ը�뾶

    [SerializeField]
    private Animator animator;
    [SerializeField]
    private MeshRenderer[] mRendenerers;
    [SerializeField]
    private Collider col;
    [SerializeField]
    private GameObject explosionEffect;
    [SerializeField]
    private float impact = 5f;

    [SerializeField]
    private float explodeDalay = 3f;    // ��ը��ʱ
    [SerializeField]
    private Transform[] explosionTrans; // ��ըλ��
    [SerializeField]
    private float explosionInterval = 0.1f; // ��ը���

    // cluster explode
    [SerializeField]
    private GameObject subGrenade;
    [SerializeField]
    private Transform[] subGrenadeTrans;
    [SerializeField]
    private float splitForce = 2f;

    private int damage;

    private bool isCountDown = false;
    private bool isExploded = false;

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isCountDown) return;
        if (other.tag == "Enemy")
        {
            isCountDown = true;
            // todo: �ƹ���������
            animator.SetTrigger("isTrigger");
            AudioManager.Instance.Play("MineDetonate");
            StartCoroutine(ExplodeAfterDelay(explodeDalay));
        }
    }

    public IEnumerator ExplodeAfterDelay(float time = 3f)
    {
        if (time == 0f) yield return null;
        else yield return new WaitForSeconds(time);

        Explode();
    }

    public void Explode()
    {
        if (isExploded) return;
        isExploded = true;

        foreach(MeshRenderer mr in mRendenerers)
        {
            mr.enabled = false;
        }
        col.enabled = false;

        // �౬
        if (explosionNums == 1)
        {
            explode(explosionTrans[0]);
        }
        else
        {
            StartCoroutine(ExplodeInOrder(explosionInterval));
        }

        // ���ѱ�
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
        GameObject explosionEffect = trans.Find("BigExplosion").gameObject;
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
