using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PickUp : MonoBehaviour
{
    [SerializeField]
    public WeaponType pickupWeaponType;
    [SerializeField]
    public int healingAmount;

    public event Action<PickUp> onPickupPicked;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            AudioManager.Instance.Play("Pickup");
            PlayerController player = other.transform.GetComponent<PlayerController>();
            player.PickupWeaponOfType(pickupWeaponType);
            player.PickupHealth(healingAmount);

            onPickupPicked?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
