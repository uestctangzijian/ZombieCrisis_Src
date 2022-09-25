using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Weapon currentWeapon; // 当前武器

    private float shotBetweenTime; // 攻击间隔

    private bool isFiring;

    private float delayAfterWeaponSwitch = 0.25f;

    public event Action<Weapon, int> OnWeaponShot;
    public event Action<Weapon> onAmmoRunout;

    // 计时，手榴弹所用
    private float startTime; 

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // 通过脚本也可以初始化对象
        if (currentWeapon)
        {
            currentWeapon = Instantiate(currentWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 射击
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFiring = true;

            startTime = Time.time;
        } 
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isFiring = false;

            // 手榴弹在攻击键抬起时扔出
            if (currentWeapon.GetWeaponDef().type == WeaponType.grenade)
            {
                GrenadeGenerator gg = currentWeapon as GrenadeGenerator;
                gg.Throw(Time.time - startTime);
                OnWeaponShot?.Invoke(currentWeapon, currentWeapon.CurAmmo);
            }
        }
    }

    private void LateUpdate()
    {
        shotBetweenTime -= Time.deltaTime;
        // 射击
        if (isFiring)
        {
            if (shotBetweenTime <= 0)
            {
                shotBetweenTime = currentWeapon.TimeBetweenShots;
                currentWeapon.Fire();
                AudioManager.Instance.PlayWeaponShotSound(currentWeapon.GetWeaponDef().type);
                OnWeaponShot?.Invoke(currentWeapon, currentWeapon.CurAmmo);

                // 子弹用尽
                if (currentWeapon.CurAmmo == 0)
                {
                    // 切换成Pistol
                    onAmmoRunout?.Invoke(currentWeapon);
                    shotBetweenTime = currentWeapon.TimeBetweenShots;
                    return;
                }
            }
        } else
        {
            if (shotBetweenTime <= 0) shotBetweenTime = 0;
        }
    }

    public Weapon GetCurWeapon()
    {
        return currentWeapon;
    }

    public void EquipWeapon(Weapon weapon)
    {
        currentWeapon.gameObject.SetActive(false);
        weapon.gameObject.SetActive(true);

        currentWeapon = weapon;
        shotBetweenTime = delayAfterWeaponSwitch;
    }
}
