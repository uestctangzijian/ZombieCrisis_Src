using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons;

    private int curWeapon = 0;

    [SerializeField]
    private Transform[] weaponTrans;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("Player").GetComponent<PlayerController>().onWeaponEquipped += UpdateCurWeapon;
    }

    // ��ʼ����
    public void InitializeWithStartWeapon(Weapon startWeapon)
    {
        weapons = new List<Weapon>();
        AddWeapon(startWeapon);
        curWeapon = 0;
    }

    // ���������
    public void AddWeapon(Weapon weapon)
    {
        if (!hasContainsWeapon(weapon))
        {
            GameObject go = weapon.gameObject;
            go.transform.parent = transform;
            go.transform.localPosition = weapon.GetWeaponDef().pos;
            go.transform.localRotation = transform.localRotation;
            if (weapon.GetWeaponDef().type != WeaponType.pistol)
                StartCoroutine(setInvisible(go, 0.01f));

            weapons.Add(weapon);
        } else 
        {
            Debug.Log("�Ѿ�ӵ������:" + weapon.GetWeaponDef().name);
        }
    }

    // ����index ��ȡָ������
    public Weapon GetWeapon(int index)
    {
        if (index < weapons.Count)
        {
            return weapons[index];
        } else
        {
            return null;
        }
    }
    
    // ����type ��ȡָ������
    public Weapon GetWeaponByType(WeaponType weaponType)
    {
        return weapons.Find(w => w.GetWeaponDef().type == weaponType);
    }

    // �Ƿ��Ѿ���������
    private bool hasContainsWeapon(Weapon weapon)
    {
        foreach (Weapon w in weapons)
        {
            if (w.GetType() == weapon.GetType())
            {
                return true;
            }
        }
        return false;
    }

    // ���µ�ǰ����
    private void UpdateCurWeapon(Weapon weapon)
    {
        curWeapon = weapons.IndexOf(weapon);
    }

    // ��������
    public void MakeWeaponUpgrade(UpgradeInfo upgrade)
    {
        Weapon weapon = GetWeaponByType(upgrade.weaponType);
        weapon.UpgradeWith(upgrade);
        weapon.RestockAmmo();
    }

    // ˢ������
    public void RestockWeaponOfType(WeaponType weaponType)
    {
        Weapon weapon = GetWeaponByType(weaponType);
        if (weapon != null)
            weapon.RestockAmmo();
        else
            Debug.LogError("���û�д�������" + weaponType);
    }

    private IEnumerator setInvisible(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);

        go.SetActive(false);
    }

    // ��ȡ��������
    public int GetWeaponsCount()
    {
        return weapons.Count;
    }
}
