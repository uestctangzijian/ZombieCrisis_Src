using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargepackGenerator : Weapon, IUpgradeAmmo, IUpgradeClusterExplode, IUpgradeExplosionNums, IUpgradeExplosionRange
{
    [SerializeField]
    private GameObject charegepackPrefab;

    [SerializeField]
    private Transform spawner;

    [SerializeField]
    private LayerMask overlapLayer;

    [SerializeField]
    private Vector3 halfEvents;

    private List<Chargepack> chargepacks;


    private void Awake()
    {
        chargepacks = new List<Chargepack>();
    }


    public override void Fire()
    {
        if (chargepacks.Count == 0)
        {
            if (!isValidPos(spawner)) return;

            GameObject go = Instantiate(charegepackPrefab) as GameObject;
            go.transform.position = spawner.position;

            Chargepack chargepack = go.GetComponent<Chargepack>();
            chargepack.Damage = def.damage;

            chargepacks.Add(chargepack);

            if (!HasUnlimitedAmmo) CurAmmo--;
        } else
        {
            AudioManager.Instance.Play("MineDetonate");
            foreach (Chargepack chargepack in chargepacks)
            {
                chargepack.Explode();
            }
            chargepacks.Clear();
        }
    }

    private bool isValidPos(Transform trans)
    {
        Collider[] hitColliders = Physics.OverlapBox(spawner.position, halfEvents, Quaternion.identity, overlapLayer);
        if (hitColliders.Length != 0)
        {
            return false;
        }
        return true;
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
        Chargepack.clusterExplodeEnable = true;
    }

    public void UpgradeExplosionNums(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Chargepack.explosionNums = (int)(Chargepack.explosionNums * value);
        else
            Chargepack.explosionNums = (int)(Chargepack.explosionNums + value);
    }

    public void UpgradeExplosionRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Chargepack.explosionRadius = (int)(Chargepack.explosionRadius * value);
        else
            Chargepack.explosionRadius = (int)(Chargepack.explosionRadius + value);
    }

    #endregion
}
