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

    // 初始武器
    public void InitializeWithStartWeapon(Weapon startWeapon)
    {
        weapons = new List<Weapon>();
        AddWeapon(startWeapon);
        curWeapon = 0;
    }

    // 添加新武器
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
            Debug.Log("已经拥有武器:" + weapon.GetWeaponDef().name);
        }
    }

    // 根据index 获取指定武器
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
    
    // 根据type 获取指定武器
    public Weapon GetWeaponByType(WeaponType weaponType)
    {
        return weapons.Find(w => w.GetWeaponDef().type == weaponType);
    }

    // 是否已经包含武器
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

    // 更新当前武器
    private void UpdateCurWeapon(Weapon weapon)
    {
        curWeapon = weapons.IndexOf(weapon);
    }

    // 升级武器
    public void MakeWeaponUpgrade(UpgradeInfo upgrade)
    {
        Weapon weapon = GetWeaponByType(upgrade.weaponType);
        weapon.UpgradeWith(upgrade);
        weapon.RestockAmmo();
    }

    // 刷新武器
    public void RestockWeaponOfType(WeaponType weaponType)
    {
        Weapon weapon = GetWeaponByType(weaponType);
        if (weapon != null)
            weapon.RestockAmmo();
        else
            Debug.LogError("玩家没有此武器：" + weaponType);
    }

    private IEnumerator setInvisible(GameObject go, float time)
    {
        yield return new WaitForSeconds(time);

        go.SetActive(false);
    }

    // 获取武器数量
    public int GetWeaponsCount()
    {
        return weapons.Count;
    }
}
