using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject pickUpPrefab;
    [SerializeField]
    private WeaponInventory inventory;

    private PickUp pickup;

    [SerializeField]
    private float pickupRefreshTime = 30f;

    private float pickupCountDown;

    void Start()
    {
        pickupCountDown = pickupRefreshTime;
        refreshItem();
    }

    void Update()
    {
        if (pickup) return;

        if (pickup == null && pickupCountDown > 0)
        {
            pickupCountDown -= Time.deltaTime;
            
        } else if (pickup == null && pickupCountDown <= 0)
        {
            pickupCountDown = 0;
            refreshItem();
        }
    }

    private void refreshItem()
    {
        this.pickup = makeItem(transform);
        this.pickup.onPickupPicked += refreshPickUp;
        this.pickup.transform.parent = transform;
    }

    public PickUp makeItem(Transform transform)
    {
        GameObject go = Instantiate(pickUpPrefab) as GameObject;
        go.transform.position = new Vector3(transform.position.x, go.transform.localScale.y / 2, transform.position.z);

        PickUp pickup = go.GetComponent<PickUp>();

        List<int> items = new List<int>();

        int weaponsCount = inventory.GetWeaponsCount();
        for (int i = 1; i <= weaponsCount; i++)
        {
            items.Add(i);
        }
        int itemIdx = Random.Range(1, items.Count);
        if (itemIdx != items.Count)
        {
            pickup.pickupWeaponType = inventory.GetWeapon(itemIdx).GetWeaponDef().type;
        }
        else
        {
            pickup.healingAmount = 100;
        }

        return pickup;
    }

    private void refreshPickUp(PickUp pickup)
    {
        pickupCountDown = pickupRefreshTime;
    }
}
