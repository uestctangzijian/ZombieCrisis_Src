using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    protected Rigidbody rb;
    protected float speed;
    protected int damage;
    protected Vector3 direction = Vector3.zero;
    protected float impact;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (direction == Vector3.zero)
        {
            direction = transform.forward;
        }
    }

    private void Update()
    {
        rb.velocity = direction * speed;
    }

    public Vector3 Direction
    {
        get
        {
            return direction;
        }
        set
        {
            direction = value;
        }
    }

    public int Damage
    {
        get
        {
            return damage;
        }
        set
        {
            damage = value;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
        set
        {
            speed = value;
        }
    }

    public float Impact
    {
        get
        {
            return impact;
        }
        set
        {
            impact = value;
        }
    }
}
