using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo
{
    public int   levelIndex;      // 第几关
    public int   enemyAmount;     // 敌人数量
    public float ememySpawnRate;  // 敌人生成速率
    public float enemyMoveSpeed;  // 敌人移动速度
    public int   bossAmount;      // boss数量
    public float bossSpawnRate;   // boss生成速率
    public float bossMoveSpeed;   // boss移动速度
}
