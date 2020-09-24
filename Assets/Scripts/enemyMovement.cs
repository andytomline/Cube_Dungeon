using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class enemyMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform Player;
    public LayerMask isGround, isPlayer;
    public float Health = 2;
    public GameObject deathEffect;
    public Transform thisEnemy;
    public float Speed;

    //Patrolling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Transform attackPoint;
    public GameObject enemyBullet;
    public float shootForce;


    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;
    public Material enemyDeathColour;

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();

    }

    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    void Patroling()
    {
        agent.speed = 2;
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkPoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.speed = 5;
        agent.SetDestination(Player.position);
        Debug.Log("Chasing Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("CollisionWithEnemy");

        if (collision.gameObject.tag.Equals("Bullet"))
        {
            Debug.Log("BulletHitEnemy");
            Health--;

            if (Health <= 0)
            {
                thisEnemy.GetComponent<MeshRenderer>().material = enemyDeathColour; 
                Invoke("enemyDeathEffect", 2);
            }
        }
    }

    void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);
        agent.speed = 0;

        Debug.Log("EnemyAttackingPlayer");
        transform.LookAt(Player);

        if (!alreadyAttacked)
        {
            //SpawningEnemyBullet
            GameObject currentBullet = Instantiate(enemyBullet, attackPoint.position, Quaternion.identity);
            Debug.Log("attackPoint Position: " + attackPoint.position);
            Debug.Log("enemy bullet position:" + enemyBullet.transform.position);
            currentBullet.GetComponent<Rigidbody>().AddForce(thisEnemy.transform.forward * shootForce * Time.deltaTime, ForceMode.VelocityChange);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    void ResetAttack()
    {
        alreadyAttacked = false;
    }

    void enemyDeathEffect()
    {
        agent.SetDestination(transform.position);
        Instantiate(deathEffect, thisEnemy.position, Quaternion.identity);
        Debug.Log(thisEnemy.position + "This enemies location");
        Destroy(this.gameObject);
    }



}

