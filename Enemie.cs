using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float detectionRadius = 10f;
    public float moveRadius = 5f;
    public float randomMoveInterval = 3f;
    public float randomMoveSpeed = 2f;
    public float health = 100f;

    private NavMeshAgent navMeshAgent;
    private Transform playerTransform;
    private Vector3 randomDestination;
    private bool isChasing = false;
    private float randomMoveTimer;
    public static Vector3 playerLocation;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (playerTransform == null)
        {
            Debug.LogError("Player with tag 'Player' not found.");
        }
        SetRandomDestination();
        randomMoveTimer = randomMoveInterval;
    } //Gets the components and sets the random destination for the enemy to move to

    private void Update()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                isChasing = true;
                ChasePlayer();
            }
            else
            {
                isChasing = false;
                Patrol();
            }
        }
    } // Updates the enemy's position and checks if the player is within the detection radius

    private void Patrol()
    {
        if (!isChasing)
        {
            randomMoveTimer -= Time.deltaTime;
            if (randomMoveTimer <= 0f)
            {
                SetRandomDestination();
                randomMoveTimer = randomMoveInterval;
            }
            MoveToDestination(randomDestination);
        }
    } // Moves the enemy to a random destination

    private void ChasePlayer()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.SetDestination(playerTransform.position);
            navMeshAgent.speed = randomMoveSpeed; 
        }
    } // Moves the enemy to the player

    private void MoveToDestination(Vector3 destination)
    {
        if (navMeshAgent != null && !isChasing)
        {
            navMeshAgent.SetDestination(destination);
            navMeshAgent.speed = randomMoveSpeed; 
        }
    } // Moves the enemy to a destination

    private void SetRandomDestination()
    {
        if (navMeshAgent != null)
        {
            Vector3 randomDirection = Random.insideUnitSphere * moveRadius;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, moveRadius, NavMesh.AllAreas);
            randomDestination = hit.position;
            MoveToDestination(randomDestination);
        }
    } // Sets a random destination for the enemy to move to
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy killed!");
            playerLocation = playerTransform.position;
            communication();
            
        }
    } // Deals damage to the enemy and destroys it if its health is 0
    public void communication()
    {
        Debug.Log("Player location is: " + playerLocation);
        gameObject.GetComponent<NavMeshAgent>().SetDestination(playerLocation);
    } // Communicates with the player to get the player's location and moves the enemy to that location
}
