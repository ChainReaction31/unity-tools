using UnityEngine;
using UnityEngine.AI;


public class ClickToMove : MonoBehaviour
{
    public Transform player;

    private float attackRange;
    private float attackSpeed;
    private float attackDamage;
    private float nextFire = 0;
    private NavMeshAgent navMeshAgent;
    private bool walking;
    private bool enemyClicked;
    private Enemy targetedEnemy;

    PlayerStats playerStats = new PlayerStats();

    void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.stoppingDistance = (attackRange - 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        //Constantly update stats
        attackDamage = GameManager.GetAttackDamage();
        attackRange = GameManager.GetAttackRange();
        attackSpeed = GameManager.GetAttackSpeed();
        navMeshAgent.stoppingDistance = (attackRange - 0.05f);

        GetTarget();
        Move();
        Attack();

        Debug.DrawLine(navMeshAgent.destination,
            new Vector3(navMeshAgent.destination.x, navMeshAgent.destination.y + 1f, navMeshAgent.destination.z),
            Color.red);
        Debug.DrawLine(navMeshAgent.transform.position, navMeshAgent.destination, Color.green);
    }


    private void GetTarget()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetButtonDown("Fire2"))
        {
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.CompareTag("Enemy"))
                {
                    targetedEnemy = hit.transform.GetComponentInParent<Enemy>();
                    enemyClicked = true;
                    navMeshAgent.destination = targetedEnemy.transform.position;
                }
                else
                {
                    walking = true;
                    enemyClicked = false;
                    navMeshAgent.destination = hit.point;
                }
            }
        }
    }

    private void Move()
    {
        if (Vector3.Distance(navMeshAgent.destination, transform.position) >= attackRange)
        {
            Debug.Log("Move range = " + attackRange);
            navMeshAgent.Resume();
            walking = true;
        }
        else if (transform.position == navMeshAgent.destination)
        {
            navMeshAgent.Stop();
        }
    }

    private void Attack()
    {
        if (targetedEnemy != null &&
            Vector3.Distance(targetedEnemy.transform.position, transform.position) <= attackRange)
        {
            transform.LookAt(targetedEnemy.transform);
            Vector3 dirToShoot = targetedEnemy.transform.position - transform.position;
            if (Time.time > nextFire)
            {
                nextFire = Time.time - attackSpeed;

                targetedEnemy.TakeDamage(attackDamage);
            }
        }
    }
}