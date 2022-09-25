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
    public int damage;              // �˺�
    public int FireRate;              // ÿ����������ӵ��� ��ʾ����
    public int ammoCapacity;          // ��������
    public float shootingRange;       // ��������
    public float velocity;            // �ӵ��ٶ�
}
