using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeGenerator : Weapon, IUpgradeClusterExplode, IUpgradeAmmo, IUpgradeExplosionNums, IUpgradeExplosionRange
{
    [SerializeField]
    private GameObject GrenadePrefab;

    [SerializeField]
    private float minVelocity;
    [SerializeField]
    private float maxVelocity;
    [SerializeField]
    private float velocityRate;

    [SerializeField]
    private Transform spawnerPos;

    public override void Fire()
    {

    }

    public void Throw(float pressDuration)
    {
        float velocity = pressDuration * velocityRate;
        if (velocity < minVelocity) velocity = minVelocity;
        if (velocity > maxVelocity) velocity = maxVelocity;

        GameObject go = Instantiate(GrenadePrefab) as GameObject;
        go.transform.position = spawnerPos.position;

        Grenade grenade = go.GetComponent<Grenade>();
        grenade.Damage = def.damage;
        grenade.Speed = transform.forward * velocity;
        if (!HasUnlimitedAmmo) CurAmmo--;
    }

    #region Interface Implementations
    public void UpgradeExplosionNums(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Grenade.explosionNums = (int)(Grenade.explosionNums * value);
        else
            Grenade.explosionNums = (int)(Grenade.explosionNums + value);
    }

    public void UpgradeClusterExplode(float value, bool isMultiplicative)
    {
        Grenade.clusterExplodeEnable = true;
    }

    public void UpgradeAmmo(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.ammoCapacity = (int)(def.ammoCapacity * value);
        else
            def.ammoCapacity = (int)(def.ammoCapacity + value);

        Debug.Log("弹夹容量提升，当前容量为：" + def.ammoCapacity);
    }

    public void UpgradeExplosionRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Grenade.explosionRadius = (int)(Grenade.explosionRadius * value);
        else
            Grenade.explosionRadius = (int)(Grenade.explosionRadius + value);
    }

    #endregion
}
