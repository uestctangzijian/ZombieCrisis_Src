using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected WeaponDefinition def;

    protected int curAmmo; // 当前子弹数

    private float timeBetweenShots; // 攻击间隔

    protected virtual void Start()
    {
        RestockAmmo();
    }

    public abstract void Fire();

    public WeaponDefinition GetWeaponDef() => def;

    /// 加满子弹
    public void RestockAmmo() => curAmmo = def.ammoCapacity;

    // 是否是放置类武器
    public bool isThrowingWeapon()
    {
        return def.type == WeaponType.barrel || def.type == WeaponType.fakeWalls || def.type == WeaponType.grenade || def.type == WeaponType.claymore || def.type == WeaponType.chargepack;
    }

    // 武器升级
    public void UpgradeWith(UpgradeInfo upgradeInfo)
    {
        switch (upgradeInfo.type)
        {
            case UpgradeType.Damage: // 伤害
                {
                    if (this is IUpgradeDamage upgradable)
                        upgradable.UpgradeDamage(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "不可以升级" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.Ammo:  // 弹夹容量
                {
                    if (this is IUpgradeAmmo upgradable)
                        upgradable.UpgradeAmmo(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "不可以升级" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.FireRate:  // 攻速
                {
                    if (this is IUpgradeFireRate upgradable)
                        upgradable.UpgradeFireRate(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "不可以升级" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.ShootingRange: // 攻击距离
                {
                    if (this is IUpgradeShootingRange upgradable)
                        upgradable.UpgradeShootingRange(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "不可以升级" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.ShootingWide: // 射击宽度
                {
                    if (this is IUpgradeShootingWide upgradable)
                        upgradable.UpgradeShootingWide(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("射击范围不可升级");
                    break;
                }
            case UpgradeType.ExplosionNums: // 爆炸次数
                {
                    if (this is IUpgradeExplosionNums upgradable)
                        upgradable.UpgradeExplosionNums(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("爆炸次数不可升级");
                    break;
                }
            case UpgradeType.ClusterExplode: // 集束炸弹
                {
                    if (this is IUpgradeClusterExplode upgradable)
                        upgradable.UpgradeClusterExplode(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("集束爆炸不可升级");
                    break;
                }
            case UpgradeType.ExplosionRange: // 爆炸范围
                {
                    if (this is IUpgradeExplosionRange upgradable)
                        upgradable.UpgradeExplosionRange(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("爆炸范围不可提升");
                    break;
                }
            default:
                {
                    Debug.LogWarning("暂时没有处理该升级类型" + upgradeInfo.type);
                    break;
                }
        }
    }

    public int CurAmmo
    {
        get
        {
            return curAmmo;
        }

        set
        {
            curAmmo--;
        }
    }

    public float TimeBetweenShots
    {
        get
        {
            timeBetweenShots = 60.0f / def.FireRate;
            return timeBetweenShots;
        }
    }

    public bool HasUnlimitedAmmo
    {
        get
        {
            return def.type == WeaponType.pistol;
        }
    }
}
