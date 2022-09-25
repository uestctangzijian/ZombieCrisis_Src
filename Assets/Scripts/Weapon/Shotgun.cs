using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shotgun : Weapon, IUpgradeFireRate, IUpgradeAmmo, IUpgradeDamage, IUpgradeShootingWide, IUpgradeShootingRange
{
    [SerializeField]
    private LineRenderer[] lineRenderers;

    [SerializeField]
    private float attackForce = 10f;

    [SerializeField]
    private float spreadAngle; // 射击宽度

    private int amountOfRays;
    private RaycastHit[] hitInfos;

    private Transform muzzleTrans;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        amountOfRays = lineRenderers.Length;
        hitInfos = new RaycastHit[amountOfRays];
        muzzleTrans = transform.Find("muzzle");
    }

    public override void Fire()
    {
        //Collider hitCollider;
        //if (isWeaponInsideBlockingObject(muzzleTrans.position, out hitCollider))
        //{
        //    HitObject(hitCollider.transform, def.damage);
        //    CurAmmo--;
        //    return;
        //}

        EnableLineRenderers(true);
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            Vector3 direction;
            if (i == 0)
            {
                direction = muzzleTrans.forward;
            } else if (i == 1)
            {
                direction = Quaternion.AngleAxis(-spreadAngle / 2.0f, Vector3.up) * muzzleTrans.forward;
            } else if (i == 2)
            {
                direction = Quaternion.AngleAxis(-spreadAngle / 4.0f, Vector3.up) * muzzleTrans.forward;
            } else if (i == 3)
            {
                direction = Quaternion.AngleAxis(spreadAngle / 4.0f, Vector3.up) * muzzleTrans.forward;
            } else if (i == 4)
            {
                direction = Quaternion.AngleAxis(spreadAngle / 2.0f, Vector3.up) * muzzleTrans.forward;
            } else
            {
                direction = Vector3.zero;
            }

            lineRenderers[i].SetPosition(0, muzzleTrans.position);
            ShootWithRay(muzzleTrans.position, direction, i);
        }
        CurAmmo--;
        StartCoroutine(DisableLineRenderersAfterDelay());
    }

    private void ShootWithRay(Vector3 origin, Vector3 direction, int index)
    {
        if (Physics.Raycast(origin, direction, out hitInfos[index], def.shootingRange))
        {
            lineRenderers[index].SetPosition(1, hitInfos[index].point);
            HitObject(hitInfos[index].transform, def.damage);
        } else
        {
            lineRenderers[index].SetPosition(1, origin + direction * def.shootingRange);
        }
    }

    private bool isWeaponInsideBlockingObject(Vector3 position, out Collider hitCollider)
    {
        Collider[] colliders = Physics.OverlapSphere(muzzleTrans.position, 0.7f);
        Collider closest = colliders.FirstOrDefault(col => col.CompareTag("Enemy") || col.CompareTag("Wall"));
        if (closest != null)
        {
            hitCollider = closest;
            return true;
        }
        hitCollider = null;
        return false;
    }

    private void HitObject(Transform hitObjectTransform, int damage)
    {
        if (hitObjectTransform.CompareTag("Enemy"))
        {
            hitObjectTransform.GetComponent<EnemyController>().TakeDamage(damage, muzzleTrans.forward * attackForce);
        }
        if (hitObjectTransform.CompareTag("Barrel"))
        {
            Barrel barrel = hitObjectTransform.transform.GetComponent<Barrel>();
            if (barrel != null)
            {
                barrel.Explode();
            }
        }
        if (hitObjectTransform.CompareTag("FakeWall"))
        {
            FakeWall fakewall = hitObjectTransform.transform.parent.GetComponent<FakeWall>();
            if (fakewall != null)
            {
                fakewall.TakeDamage(def.damage);
            }
        }
    }

    private void EnableLineRenderers(bool enabled)
    {
        for (int i = 0; i < lineRenderers.Length; i++)
        {
            lineRenderers[i].enabled = enabled;
        }
    }

    private IEnumerator DisableLineRenderersAfterDelay(float time = 0.1f)
    {
        if (time == 0.0f) yield return null;
        else yield return new WaitForSeconds(time);

        EnableLineRenderers(false);
    }

    #region Interface Implementations

    public void UpgradeDamage(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.damage = (int)(def.damage * value);
        else
            def.damage = (int)(def.damage + value);

        Debug.Log("伤害提升,当前伤害为：" + def.damage);
    }

    public void UpgradeFireRate(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.FireRate = (int)(def.FireRate * value);
        else
            def.FireRate = (int)(def.FireRate + value);

        Debug.Log("射速提升，当前射速为：" + def.FireRate);
    }

    public void UpgradeAmmo(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.ammoCapacity = (int)(def.ammoCapacity * value);
        else
            def.ammoCapacity = (int)(def.ammoCapacity + value);

        Debug.Log("弹夹容量提升，当前容量为：" + def.ammoCapacity);
    }

    public void UpgradeShootingWide(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            spreadAngle = spreadAngle * value;
        else
            spreadAngle = spreadAngle + value;

        Debug.Log("射击范围提升, 当前容量为：" + spreadAngle);
    }

    public void UpgradeShootingRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.shootingRange = (int)(def.shootingRange * value);
        else
            def.shootingRange = (int)(def.shootingRange + value);

        Debug.Log("射击距离提升， 当前距离为：" + def.shootingRange);
    }

    #endregion
}


