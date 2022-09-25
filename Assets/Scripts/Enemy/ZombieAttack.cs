using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAttack : EnemyAttack
{

    private float impactRadius = 1.5f;

    public override void attack(Vector3 attackDirection)
    {
        Collider[] hitObjects = Physics.OverlapSphere(damageArea.position, impactRadius);
        for (int i = 0; i < hitObjects.Length; i++)
        {
            if (hitObjects[i].CompareTag("Player"))
            {
                hitObjects[i].transform.GetComponent<PlayerHealth>().TakeDamage(attackDamage, attackDiretion * impact);
            }
        }
    }
}
