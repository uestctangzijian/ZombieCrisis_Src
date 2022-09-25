using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    private KeyCode[] numKeyCodes = new KeyCode[] { KeyCode.Alpha0, KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6, KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9 };

    [SerializeField]
    public PlayerAttack attack;
    [SerializeField]
    private PlayerHealth health;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private WeaponInventory inventory; // ËùÓÐÎäÆ÷
    [SerializeField]
    private ScoreManager score;
    [SerializeField]
    private LevelManager levelManager;

    // Î¯ÍÐ
    public event Action<Weapon> onWeaponEquipped;
    public event Action<EnemyController> onEnemyKilled;
    public event Action<Weapon> onWeaponUnlocked;
    public event Action<Weapon, UpgradeInfo> onWeaponUpgrade;
    public event Action<Weapon> onPickUp;

    // Start is called before the first frame update
    void Start()
    {
        inventory.InitializeWithStartWeapon(attack.GetCurWeapon());
        attack.onAmmoRunout += SwitchDefaultWeapon;
        health.onDealth += Dead;
    }

    // Update is called once per frame
    void Update()
    {
        // ÇÐ»»ÎäÆ÷
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                SwitchWeapon(9);
            } else
            {
                for (int i = (int)KeyCode.Alpha1; i <= (int)KeyCode.Alpha9; i++)
                {
                    if (Input.GetKeyDown((KeyCode)i))
                    {
                        SwitchWeapon((i - (int)KeyCode.Alpha1));
                    }
                }
            }
        }
    }

    // »÷É±µÐÈË
    public void KilledEnemy(EnemyController enemy)
    {
        score.IncreseScore(enemy.points);
        onEnemyKilled?.Invoke(enemy);
    }

    // ½âËøÎäÆ÷
    public void UnlockWeapon(Weapon weapon)
    {
        inventory.AddWeapon(weapon);
        onWeaponUnlocked?.Invoke(weapon);
    }

    // ÇÐ»»ÎäÆ÷
    public void SwitchWeapon(int index)
    {
        Weapon weapon = inventory.GetWeapon(index);
        if (weapon != null && attack.GetCurWeapon() != weapon && weapon.CurAmmo != 0)
        {
            Debug.Log(weapon.GetWeaponDef().name);
            attack.EquipWeapon(weapon);
            onWeaponEquipped?.Invoke(weapon);

            Transform rightHand = gameObject.transform.Find("Models/Right_Hand/Arm_and_hand");

            if (weapon.isThrowingWeapon())
            {
                animator.SetBool("unheld", true);
            }
            else
            {
                animator.SetBool("unheld", false);
            }
        }
    }

    public void SwitchDefaultWeapon(Weapon weapon)
    {
        SwitchWeapon(0);
    }

    // Éý¼¶ÎäÆ÷
    public void MakeWeaponUpgrade(UpgradeInfo upgrade)
    {
        inventory.MakeWeaponUpgrade(upgrade);
        onWeaponUpgrade?.Invoke(inventory.GetWeaponByType(upgrade.weaponType), upgrade);
    }

    // Ê°È¡ÎäÆ÷
    public void PickupWeaponOfType(WeaponType weaponType)
    {
        inventory.RestockWeaponOfType(weaponType);

        Weapon pickedUpWeapon = inventory.GetWeaponByType(weaponType);
        if (pickedUpWeapon != null)
            onPickUp?.Invoke(pickedUpWeapon);
    }

    public void PickupHealth(int hp)
    {
        health.Heal(hp);
    }

    private void Dead(int hp)
    {
        levelManager.GameFinished("You Die!");
    }
}
