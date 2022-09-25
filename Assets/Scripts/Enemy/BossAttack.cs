using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : EnemyAttack
{
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileScale = 1f;
    [SerializeField]
    private LayerMask DestructibleLayers;
    

    public override void attack(Vector3 attackDirection)
    {
        BossFireball fireball = Instantiate(projectilePrefab, damageArea.position, Quaternion.identity).GetComponent<BossFireball>();
        fireball.Speed = projectileSpeed;
        fireball.Damage = attackDamage;
        fireball.transform.localScale = fireball.transform.localScale * projectileScale;
        fireball.Direction = attackDirection;
        fireball.Impact = impact;
    }

    // boss¿ÉÒÔ¹¥»÷fakeWall
    protected override bool isBlockByFakeWall()
    {
        LayerMask mask = ~(1 << LayerMask.NameToLayer("Fireball"));

        if (Physics.Raycast(damageArea.position, damageArea.forward, out RaycastHit hit, attackRange, mask))
        {
            return hit.transform.CompareTag("FakeWall");
        }
        return false;

        //Vector3[] corners = new Vector3[2];
        //int length = agent.path.GetCornersNonAlloc(corners);
        //if (length > 1 && Physics.Raycast(corners[0], (corners[1] - corners[0]).normalized, out RaycastHit hit, attackRange, DestructibleLayers))
        //{
        //    return true;
        //}

        //return false;
    }
}
