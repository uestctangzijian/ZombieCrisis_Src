using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.pistol;
    public int index;
    public Vector3 pos;

    public string name;
    public int damage;              // 伤害
    public int FireRate;              // 每分钟射出的子弹数 表示射速
    public int ammoCapacity;          // 弹夹容量
    public float shootingRange;       // 攻击距离
    public float velocity;            // 子弹速度
}
