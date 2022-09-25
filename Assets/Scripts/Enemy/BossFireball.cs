using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireball : Fireball
{
    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealth health = collision.transform.GetComponent<PlayerHealth>();
        if (health != null)
        {
            health.TakeDamage(damage, direction * impact);
        }
        Destroy(gameObject);

        if (collision.transform.CompareTag("FakeWall"))
        {
            FakeWall fakewall = collision.transform.parent.GetComponent<FakeWall>();
            if (fakewall != null)
            {
                fakewall.TakeDamage(damage);
            }
        }
    }
}
