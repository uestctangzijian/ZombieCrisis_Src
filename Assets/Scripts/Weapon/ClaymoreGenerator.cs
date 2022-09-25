using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreGenerator : Weapon, IUpgradeAmmo, IUpgradeClusterExplode, IUpgradeExplosionNums, IUpgradeExplosionRange
{
    [SerializeField]
    private GameObject claymorePrefab;

    [SerializeField]
    private Transform spawnerPos;

    [SerializeField]
    private LayerMask overlapLayer;

    [SerializeField]
    private Vector3 halfEvents;

    public override void Fire()
    {
        if (!isValidPos(spawnerPos)) return;
        
        GameObject go = Instantiate(claymorePrefab) as GameObject;
        go.transform.position = spawnerPos.position;

        Claymore claymore = go.GetComponent<Claymore>();
        claymore.Damage = def.damage;

        if (!HasUnlimitedAmmo) CurAmmo--;
    }

    private bool isValidPos(Transform trans)
    {
        Collider[] hitColliders = Physics.OverlapBox(spawnerPos.position, halfEvents, Quaternion.identity, overlapLayer);
        if (hitColliders.Length != 0)
        {
            return false;
        }
        return true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(spawnerPos.position, halfEvents);
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

    public void UpgradeClusterExplode(float value, bool isMultiplicative)
    {
        Claymore.clusterExplodeEnable = true;
    }

    public void UpgradeExplosionNums(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Claymore.explosionNums = (int)(Claymore.explosionNums * value);
        else
            Claymore.explosionNums = (int)(Claymore.explosionNums + value);
    }

    public void UpgradeExplosionRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Claymore.explosionRadius = (int)(Claymore.explosionRadius * value);
        else
            Claymore.explosionRadius = (int)(Claymore.explosionRadius + value);
    }

    #endregion
}
