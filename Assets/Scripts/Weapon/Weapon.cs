using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField]
    protected WeaponDefinition def;

    protected int curAmmo; // ��ǰ�ӵ���

    private float timeBetweenShots; // �������

    protected virtual void Start()
    {
        RestockAmmo();
    }

    public abstract void Fire();

    public WeaponDefinition GetWeaponDef() => def;

    /// �����ӵ�
    public void RestockAmmo() => curAmmo = def.ammoCapacity;

    // �Ƿ��Ƿ���������
    public bool isThrowingWeapon()
    {
        return def.type == WeaponType.barrel || def.type == WeaponType.fakeWalls || def.type == WeaponType.grenade || def.type == WeaponType.claymore || def.type == WeaponType.chargepack;
    }

    // ��������
    public void UpgradeWith(UpgradeInfo upgradeInfo)
    {
        switch (upgradeInfo.type)
        {
            case UpgradeType.Damage: // �˺�
                {
                    if (this is IUpgradeDamage upgradable)
                        upgradable.UpgradeDamage(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "����������" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.Ammo:  // ��������
                {
                    if (this is IUpgradeAmmo upgradable)
                        upgradable.UpgradeAmmo(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "����������" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.FireRate:  // ����
                {
                    if (this is IUpgradeFireRate upgradable)
                        upgradable.UpgradeFireRate(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "����������" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.ShootingRange: // ��������
                {
                    if (this is IUpgradeShootingRange upgradable)
                        upgradable.UpgradeShootingRange(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError(def.name + "����������" + upgradeInfo.type);
                    break;
                }
            case UpgradeType.ShootingWide: // ������
                {
                    if (this is IUpgradeShootingWide upgradable)
                        upgradable.UpgradeShootingWide(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("�����Χ��������");
                    break;
                }
            case UpgradeType.ExplosionNums: // ��ը����
                {
                    if (this is IUpgradeExplosionNums upgradable)
                        upgradable.UpgradeExplosionNums(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("��ը������������");
                    break;
                }
            case UpgradeType.ClusterExplode: // ����ը��
                {
                    if (this is IUpgradeClusterExplode upgradable)
                        upgradable.UpgradeClusterExplode(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("������ը��������");
                    break;
                }
            case UpgradeType.ExplosionRange: // ��ը��Χ
                {
                    if (this is IUpgradeExplosionRange upgradable)
                        upgradable.UpgradeExplosionRange(upgradeInfo.value, upgradeInfo.isMultiplicativeValue);
                    else
                        Debug.LogError("��ը��Χ��������");
                    break;
                }
            default:
                {
                    Debug.LogWarning("��ʱû�д������������" + upgradeInfo.type);
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
