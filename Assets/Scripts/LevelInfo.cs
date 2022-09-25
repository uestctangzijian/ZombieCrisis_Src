using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelInfo
{
    public int   levelIndex;      // �ڼ���
    public int   enemyAmount;     // ��������
    public float ememySpawnRate;  // ������������
    public float enemyMoveSpeed;  // �����ƶ��ٶ�
    public int   bossAmount;      // boss����
    public float bossSpawnRate;   // boss��������
    public float bossMoveSpeed;   // boss�ƶ��ٶ�
}
