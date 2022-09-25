using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponText : MonoBehaviour
{
    private TextMeshProUGUI weaponNameText;

    private string currentWeaponName;

    [SerializeField]
    private PlayerController player;
    private PlayerAttack playerAttack;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        playerAttack = player.GetComponent<PlayerAttack>();
        playerHealth = player.GetComponent<PlayerHealth>();
    }
    // Start is called before the first frame update
    void Start()
    {
        playerHealth.onDealth += hide;
    }

    // Update is called once per frame
    void Update()
    {
        weaponNameText = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        player.onWeaponEquipped += UpdateWeaponName;
        playerAttack.OnWeaponShot += UpdateWeaponAmmo;
    }

    private void UpdateWeaponName(Weapon weapon)
    {
        currentWeaponName = weapon.GetWeaponDef().name;
        if (weapon.HasUnlimitedAmmo)
        {
            weaponNameText.text = currentWeaponName;
        } else
        {
            weaponNameText.text = string.Format("{0}:{1}", currentWeaponName, weapon.CurAmmo);
        }
    }

    private void UpdateWeaponAmmo(Weapon weapon, int ammo)
    {
        if (currentWeaponName == null)
        {
            currentWeaponName = weapon.GetWeaponDef().name;
        }
        if (weapon.HasUnlimitedAmmo)
        {
            weaponNameText.text = weapon.GetWeaponDef().name;
        } else
        {
            weaponNameText.text = string.Format("{0}:{1}", currentWeaponName, ammo);
        }
    }

    private void hide(int health)
    {
        weaponNameText.gameObject.SetActive(false);
    }
}
