using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Railgun : Weapon, IUpgradeFireRate, IUpgradeAmmo, IUpgradeShootingRange
{
    [SerializeField]
    private LineRenderer gunLine;

    private float attackForce = 5f;

    [SerializeField]
    private float lineDuration = 0.1f; // 射线存在时长

    [SerializeField]
    private LayerMask blockLayer;

    [SerializeField]
    private Transform muzzle;

    public override void Fire()
    {
        Ray ray = new Ray(muzzle.position, transform.forward);

        EnableLineRenderer(true);
        gunLine.SetPosition(0, transform.position);

        bool hasSetPos1 = false;

        // raycastAll 返回的hits数组元素顺序不定,所以要排序
        // https://answers.unity.com/questions/282165/raycastall-returning-results-in-reverse-order-of-c-1.html
        RaycastHit[] hits = Physics.RaycastAll(ray, def.shootingRange).OrderBy(h =>h.distance).ToArray();
        foreach (RaycastHit h in hits)
        {
            Transform rootT = h.collider.gameObject.transform;
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
            if (otherGO.tag == "Wall")
            {
                hasSetPos1 = true;
                gunLine.SetPosition(1, h.point);
                break;
            }
        }
        if (!hasSetPos1) gunLine.SetPosition(1, ray.origin + ray.direction * def.shootingRange);

        curAmmo--;
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

    public void UpgradeAmmo(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.ammoCapacity = (int)(def.ammoCapacity * value);
        else
            def.ammoCapacity = (int)(def.ammoCapacity + value);

        Debug.Log("弹夹容量提升，当前容量为：" + def.ammoCapacity);
    }

    public void UpgradeShootingRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.shootingRange = (int)(def.shootingRange * value);
        else
            def.shootingRange = (int)(def.shootingRange + value);

        Debug.Log("攻击距离提升,当前距离为：" + def.shootingRange);
    }

    #endregion
}
