using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTrace : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 1f;
    [SerializeField] private float backSpeed = 2f;

    [SerializeField]
    public NavMeshAgent agent;
    [SerializeField]
    private Animator animator;

    public Transform target;

    public bool isAlive;


    // Start is called before the first frame update
    void Start()
    {
        agent.speed = movementSpeed;
        agent.SetDestination(target.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

        //agent.SetDestination(target.position);

    }

    private void LateUpdate()
    {
        animator.SetFloat("speed", Vector3.Magnitude(agent.velocity));
    }

    public void Knockback(Vector3 force)
    {
        // »÷ÍË
        agent.velocity = -transform.forward * backSpeed;
        animator.SetBool("attack", false);
        animator.SetTrigger("isAttacked");
    }

    void OnDrawGizmosSelected()
    {

        var nav = GetComponent<NavMeshAgent>();
        if (nav == null || nav.path == null)
            return;

        var line = this.GetComponent<LineRenderer>();
        if (line == null)
        {
            line = this.gameObject.AddComponent<LineRenderer>();
            line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
            line.SetWidth(0.5f, 0.5f);
            line.SetColors(Color.yellow, Color.yellow);
        }

        var path = nav.path;

        line.SetVertexCount(path.corners.Length);

        for (int i = 0; i < path.corners.Length; i++)
        {
            line.SetPosition(i, path.corners[i]);
        }

    }
}
