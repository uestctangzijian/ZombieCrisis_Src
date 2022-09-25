using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInfo
{
    public WeaponType weaponType;       // 武器类型
    public UpgradeType type;            // 升级类型
    public float value;                 // 提升值
    public bool isMultiplicativeValue;  // 是否倍增
    public int UnlockMultiplier;        // 多少连杀数解锁
    public string DisplayName;          // 展示名
}
