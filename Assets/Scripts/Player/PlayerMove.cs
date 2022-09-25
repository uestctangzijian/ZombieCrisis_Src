using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotSpeed = 15.0f;

    private Rigidbody rb;
    private Animator animator;
    private PlayerHealth playerHealth;

    private bool movingUp;
    private bool movingRight;

    private bool isAttacked;

    [SerializeField]
    private float xMin = -18.5f;
    [SerializeField]
    private float xMax = 7f;
    [SerializeField]
    private float zMin = -25f;
    [SerializeField]
    private float zMax = 14f;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Update is called once per frame
    void Update()
    {
        //float deltax = Input.GetAxis("Horizontal") * speed;
        //float deltaz = Input.GetAxis("Vertical") * speed;

        // 移动方法一，设置速度
        //Vector3 velocity = new Vector3(deltax, 0, deltaz);
        //rigidbody.velocity = velocity;
        //animator.SetFloat("speed", Vector3.Magnitude(velocity));
        //debug.log("speed:" + vector3.magnitude(velocity));

        // 移动方法二
        //Vector3 targetpos = this.transform.position + new Vector3(deltax, 0, deltaz) * Time.deltaTime;
        //rigidbody.MovePosition(targetpos);

        //if (deltax != 0 || deltaz != 0)
        //{
        //    // 转向
        //    Vector3 targetdirection = new Vector3(deltax, 0, deltaz);
        //    Quaternion targetrotation = Quaternion.LookRotation(targetdirection, Vector3.up);

        //    Quaternion newrotation = Quaternion.Lerp(rigidbody.rotation, targetrotation, rotSpeed * Time.deltaTime);
        //    rigidbody.MoveRotation(targetrotation);
        //}

        if (!playerHealth.isAlive) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 1) movingUp = true;
        else if (horizontal == -1) movingUp = false;

        if (vertical == 1) movingRight = true;
        else if (vertical == -1) movingRight = false;

        if (horizontal == 0)
        {
            if (movingRight && Input.GetKey("a"))
            {
                horizontal = -1;
            }
            else if (!movingRight && Input.GetKey("d"))
            {
                horizontal = 1;
            }
        }

        if (vertical == 0)
        {
            if (movingUp && Input.GetKey("s"))
            {
                vertical = -1;
            }
            else if (!movingUp && Input.GetKey("d"))
            {
                vertical = 1;
            }
        }

        if (!isAttacked)
        {
            Vector3 direction = new Vector3(horizontal, 0, vertical);
            Vector3 velocity = direction * speed;

            velocity = Vector3.ClampMagnitude(velocity, speed);
            rb.velocity = velocity;
            // 行走动画
            animator.SetFloat("speed", Vector3.Magnitude(velocity));

            transform.LookAt(transform.position + direction);

            transform.position = new Vector3(Mathf.Clamp(transform.position.x, xMin, xMax),
                                       transform.position.y,
                                       Mathf.Clamp(transform.position.z, zMin, zMax));
        }
    }

    public void knockback(Vector3 attackForce)
    {
        rb.AddForce(attackForce, ForceMode.Impulse);
        transform.LookAt(transform.position - attackForce.normalized);
        animator.SetBool("isAttacked", true);
        isAttacked = true;
    }

    public void AttackFinish()
    {
        animator.SetBool("isAttacked", false);
        isAttacked = false;
    }
}
