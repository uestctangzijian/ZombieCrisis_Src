using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelGenerator : Weapon, IUpgradeAmmo, IUpgradeExplosionNums, IUpgradeExplosionRange
{
    [SerializeField]
    private GameObject BarrelPrefab;

    [SerializeField]
    private Transform[] spawnerTrans;

    [SerializeField]
    private LayerMask excludeLayer;

    [SerializeField]
    private float colliderRadius = 0.5f;

    [SerializeField]
    private Transform parent;

    private bool isOverlapBarrel;

    public override void Fire()
    {
        if (isOverlapBarrel) return;
        Transform spawnerTrans = getValidSpawnerTrans();
        if (getValidSpawnerTrans() == null) return;

        isOverlapBarrel = true;
        GameObject go = Instantiate(BarrelPrefab) as GameObject;
        go.transform.position = spawnerTrans.position;
        go.transform.parent = GameObject.Find("Building/Barrels").transform;
        Barrel barrel = go.GetComponent<Barrel>();
        barrel.damage = def.damage;
        barrel.onBarrelLeavePlayer += changeOverlap;
        if (!HasUnlimitedAmmo) CurAmmo--;

        NavMeshManager.Instance.BakeNavMesh();
    }

    private Transform getValidSpawnerTrans()
    {
        foreach (Transform st in spawnerTrans)
        {
            if (isValidTrans(st))
                return st;
        }
        return null;
    }

    private bool isValidTrans(Transform trans)
    {
        Collider[] hitColliders = Physics.OverlapSphere(trans.position, colliderRadius, ~excludeLayer);
        if (hitColliders.Length > 0)
        {
            return false;
        }
        return true;
    }

    private void changeOverlap(Barrel barrel)
    {
        isOverlapBarrel = false;
    }

    #region Interface Implementations
    public void UpgradeAmmo(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.ammoCapacity = (int)(def.ammoCapacity * value);
        else
            def.ammoCapacity = (int)(def.ammoCapacity + value);

        Debug.Log("弹夹容量提升，当前容量为：" + def.ammoCapacity);
    }

    public void UpgradeExplosionNums(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Barrel.explosionNums = (int)(Barrel.explosionNums * value);
        else
            Barrel.explosionNums = (int)(Barrel.explosionNums + value);
    }

    public void UpgradeExplosionRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Barrel.explosionRadius = (int)(Barrel.explosionRadius * value);
        else
            Barrel.explosionRadius = (int)(Barrel.explosionRadius + value);
    }

    #endregion
}
