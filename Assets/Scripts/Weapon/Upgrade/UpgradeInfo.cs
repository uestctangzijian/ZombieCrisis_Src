using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UpgradeInfo
{
    public WeaponType weaponType;       // ��������
    public UpgradeType type;            // ��������
    public float value;                 // ����ֵ
    public bool isMultiplicativeValue;  // �Ƿ���
    public int UnlockMultiplier;        // ������ɱ������
    public string DisplayName;          // չʾ��
}
