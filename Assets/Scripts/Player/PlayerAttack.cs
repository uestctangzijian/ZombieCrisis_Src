using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField]
    private Weapon currentWeapon; // ��ǰ����

    private float shotBetweenTime; // �������

    private bool isFiring;

    private float delayAfterWeaponSwitch = 0.25f;

    public event Action<Weapon, int> OnWeaponShot;
    public event Action<Weapon> onAmmoRunout;

    // ��ʱ����������
    private float startTime; 

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        // ͨ���ű�Ҳ���Գ�ʼ������
        if (currentWeapon)
        {
            currentWeapon = Instantiate(currentWeapon);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // ���
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFiring = true;

            startTime = Time.time;
        } 
        if (Input.GetKeyUp(KeyCode.Space))
        {
            isFiring = false;

            // �����ڹ�����̧��ʱ�ӳ�
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
        // ���
        if (isFiring)
        {
            if (shotBetweenTime <= 0)
            {
                shotBetweenTime = currentWeapon.TimeBetweenShots;
                currentWeapon.Fire();
                AudioManager.Instance.PlayWeaponShotSound(currentWeapon.GetWeaponDef().type);
                OnWeaponShot?.Invoke(currentWeapon, currentWeapon.CurAmmo);

                // �ӵ��þ�
                if (currentWeapon.CurAmmo == 0)
                {
                    // �л���Pistol
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
