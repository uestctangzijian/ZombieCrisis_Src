using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType
{
    Damage,         // 伤害
    Ammo,           // 弹夹容量
    FireRate,       // 射速
    ShootingRange,  // 攻击距离
    UnlockWeapon,   // 新武器解锁
    ShootingWide,   // 射击宽度 （for霰弹枪）
    ExplosionNums,  // 爆炸次数 （for爆炸类武器）
    ClusterExplode, // 集束炸弹 （for爆炸类武器）
    ExplosionRange, // 爆炸范围 （for爆炸类武器）
}
