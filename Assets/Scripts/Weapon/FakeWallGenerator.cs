using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeWallGenerator : Weapon, IUpgradeAmmo
{
    [SerializeField]
    private GameObject fakeWallPrefab;

    [SerializeField]
    private Transform[] spawnerTrans;

    [SerializeField]
    private LayerMask excludeLayer;

    [SerializeField]
    private float colliderRadius = 0.5f;

    private bool isOverlapFakeWall;

    private Vector3 postion;

    public override void Fire()
    {
        if (isOverlapFakeWall) return;
        Transform spawnerTrans = getValidSpawnerTrans();
        if (getValidSpawnerTrans() == null) return;

        postion = spawnerTrans.position;

        isOverlapFakeWall = true;
        GameObject go = Instantiate(fakeWallPrefab) as GameObject;
        go.transform.position = spawnerTrans.position;
        go.transform.parent = GameObject.Find("Building/FakeWalls").transform;
        FakeWall fakewWall = go.GetComponent<FakeWall>();
        fakewWall.onFakeWallLeavePlayer += changeOverlap;
        if (!HasUnlimitedAmmo) CurAmmo--;

        NavMeshManager.Instance.BakeNavMesh();
    }

    private Transform getValidSpawnerTrans()
    {
        foreach (Transform st in spawnerTrans)
        {
            if (isValidTrans(st))
                return st;
        }
        return null;
    }

    private bool isValidTrans(Transform trans)
    {
        Collider[] hitColliders = Physics.OverlapSphere(trans.position, colliderRadius, ~excludeLayer);
        if (hitColliders.Length > 0)
        {
            return false;
        }
        return true;
    }

    private void changeOverlap(FakeWall fakewall)
    {
        isOverlapFakeWall = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(postion, colliderRadius);
    }

    #region Interface Implementations
    public void UpgradeAmmo(float value, bool isMultiplicative)
    {
        if (isMultiplicative)
            def.ammoCapacity = (int)(def.ammoCapacity * value);
        else
            def.ammoCapacity = (int)(def.ammoCapacity + value);

        Debug.Log("弹夹容量提升，当前容量为：" + def.ammoCapacity);
    }

    #endregion
}
