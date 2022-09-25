using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon, IUpgradeFireRate, IUpgradeAmmo, IUpgradeExplosionNums, IUpgradeExplosionRange
{
    [SerializeField]
    private GameObject rocketPrefab;

    [SerializeField]
    private Transform muzzleTrans;

    public override void Fire()
    {
        GameObject go = Instantiate(rocketPrefab) as GameObject;
        go.transform.position = muzzleTrans.position;
        Rocket rocket = go.GetComponent<Rocket>();
        rocket.Direction = muzzleTrans.forward;
        rocket.Speed = def.velocity;
        rocket.Damage = def.damage;
        if (!HasUnlimitedAmmo)
            CurAmmo--;
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

    public void UpgradeExplosionNums(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Rocket.explosionNums = (int)(Rocket.explosionNums * value);
        else
            Rocket.explosionNums = (int)(Rocket.explosionNums + value);

        Debug.Log("爆炸次数提升，当前次数为：" + Rocket.explosionNums);
    }

    public void UpgradeExplosionRange(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            Rocket.explosionRadius = (int)(Rocket.explosionRadius * value);
        else
            Rocket.explosionRadius = (int)(Rocket.explosionRadius + value);

        Debug.Log("爆炸范围提升，当前范围为：" + Rocket.explosionNums);
    }

    #endregion
}
