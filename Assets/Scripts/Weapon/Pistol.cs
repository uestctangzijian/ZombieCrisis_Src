using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pistol : Weapon, IUpgradeFireRate, IUpgradeDamage
{
    private LineRenderer gunLine;

    private GameObject muzzle; //枪口

    private float attackForce = 5f;

    [SerializeField]
    private float lineDuration = 0.1f; // 射线存在时长


    private void Awake()
    {
        muzzle = transform.Find("muzzle").gameObject;

        gunLine = muzzle.GetComponent<LineRenderer>();
    }

    public override void Fire()
    {
        // 创建射击射线
        Ray ray = new Ray(muzzle.transform.position, muzzle.transform.forward);
        
        RaycastHit hit;

        if (gunLine != null)
        {
            EnableLineRenderer(true);
            gunLine.SetPosition(0, muzzle.transform.position);
        }
        if (Physics.Raycast(ray, out hit, def.shootingRange)) {
            Transform rootT = hit.collider.gameObject.transform;
            GameObject otherGO = rootT.gameObject;
            if (otherGO.tag == "Enemy")
            {
                EnemyController enemy = otherGO.GetComponent<EnemyController>();
                enemy.TakeDamage(def.damage, ray.direction * attackForce);
            }
            if (otherGO.tag == "Barrel")
            {
                Barrel barrel = otherGO.transform.GetComponent<Barrel>();
                if (barrel != null)
                {
                    barrel.Explode();
                }
            }
            if (otherGO.tag == "FakeWall")
            {
                FakeWall fakewall = otherGO.transform.parent.GetComponent<FakeWall>();
                if (fakewall != null)
                {
                    fakewall.TakeDamage(def.damage);
                }
            }
            gunLine.SetPosition(1, hit.point);
        } else if (gunLine != null)
        {
            gunLine.SetPosition(1, ray.origin + ray.direction * def.shootingRange);
        }

        StartCoroutine(DisableLineRenderersAfterDelay(lineDuration));
    }

    private IEnumerator DisableLineRenderersAfterDelay(float time = 0.1f)
    {
        if (time == 0.0f) yield return null;
        else yield return new WaitForSeconds(time);

        EnableLineRenderer(false);
    }

    private void EnableLineRenderer(bool enabled)
    {
        gunLine.enabled = enabled;
    }

    #region Interface Implementations
    public void UpgradeFireRate(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.FireRate = (int)(def.FireRate * value);
        else
            def.FireRate = (int)(def.FireRate + value);

        Debug.Log("射速提升，当前射速为：" + def.FireRate);
    }

    public void UpgradeDamage(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.damage = (int)(def.damage * value);
        else
            def.damage = (int)(def.damage + value);

        Debug.Log("伤害提升,当前伤害为：" + def.damage);
    }

    #endregion
}
