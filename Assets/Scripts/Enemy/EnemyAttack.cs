using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour
{
    public Transform target;

    protected NavMeshAgent agent;
    protected Animator animator;

    [SerializeField]
    protected int attackDamage = 1;    // ¹¥»÷Á¦
    [SerializeField]
    protected float attackRange = 1.75f; // ¹¥»÷·¶Î§
    [SerializeField]
    protected float impact = 20f;      // ³å»÷Á¦
    [SerializeField]
    protected Transform damageArea;     // ¹¥»÷Î»ÖÃ

    protected Vector3 attackDiretion;

    public bool isAlive;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!isAlive) return;

        float distance = Vector3.Distance(transform.position, target.transform.position);
        bool isInAttackRange = distance <= attackRange;
        if ((isInAttackRange && isFaceToTarget()) || isBlockByFakeWall())
        {
            agent.isStopped = true;
            animator.SetBool("attack", true);
        } else
        {
            if (agent.isStopped)
                agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            Debug.Log(agent.destination);
            animator.SetBool("attack", false);
        }

        Debug.Log(agent.isStopped);
    }

    protected bool isFaceToTarget()
    {
        LayerMask mask = ~(1 << LayerMask.NameToLayer("Fireball"));

        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, attackRange, mask))
        {
            attackDiretion = transform.forward;
            return hit.transform.CompareTag("Player");
        }
        return false;
    }

    protected virtual bool isBlockByFakeWall() { return false; }

    public void AttackFromAnimation()
    {
        attack(attackDiretion);
    }

    public virtual void attack(Vector3 attackDirection)
    {

    }

    public void OnDrawGizmosSelected()
    {
        //var line = this.GetComponent<LineRenderer>();
        //if (line == null)
        //{
        //    line = this.gameObject.AddComponent<LineRenderer>();
        //    line.material = new Material(Shader.Find("Sprites/Default")) { color = Color.yellow };
        //    line.SetWidth(0.5f, 0.5f);
        //    line.SetColors(Color.yellow, Color.yellow);
        //}

        //var path = agent.path;

        //line.positionCount = path.corners.Length;

        //for (int i = 0; i < path.corners.Length; i++)
        //{
        //    line.SetPosition(i, path.corners[i]);
        //}
    }

}
