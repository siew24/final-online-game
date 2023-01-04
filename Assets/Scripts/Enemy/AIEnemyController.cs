using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GameStartListener))]
public class AIEnemyController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;

    // Speed at which the AI enemy moves
    public float speed = 2f;
    // List of waypoints for the AI enemy to patrol
    public Transform[] waypoints;
    // Index of the current waypoint the AI enemy is moving towards
    int currentWaypointIndex = 0;
    // Flag to indicate whether the AI enemy is attacking the player
    bool isAttacking = false;
    // Range at which the AI enemy will chase the player
    public float chaseRange = 10f;
    // Timer to delay the AI enemy's movement to the next waypoint
    float waypointTimer = 0f;
    // Reference to the player game object
    // Particle system for the muzzle flash
    public ParticleSystem muzzleFlash;
    // Reference to the projectile prefab
    //public GameObject muzzlePrefab;
    // Reference to the shooting point object
    public Transform shootingPoint;
    // Interval at which the AI enemy shoots projectiles
    public float shootingInterval = 1f;
    // Timer to track the shooting interval
    float shootingTimer = 0f;
    // Layer mask for the player
    public LayerMask playerLayerMask;
    // Timer to delay the AI's decision to return to patrolling after the player is out of range
    float attackTimer = 0f;

    GameStartListener gameStartListener;

    private bool _isGameStarted;

    private GameObject[] players;
    private GameObject targetedPlayer;

    void Start()
    {
        _isGameStarted = false;
        gameStartListener = GetComponent<GameStartListener>();
        gameStartListener.Register(OnGameStart);

    }

    void Update()
    {
        if (!_isGameStarted)
            return;

        if (!isAttacking)
            HandleIdle();
        else
            HandleAggressive();

        // Decrement the attack timer
        attackTimer -= Time.deltaTime;
        // If the attack timer has expired, return to patrolling
        if (attackTimer <= 0)
        {
            isAttacking = false;
        }
    }

    void OnGameStart()
    {
        // When all players are loaded into the scene,
        // populate `players` with the player objects
        players = GameObject.FindGameObjectsWithTag("Player");

        _isGameStarted = true;
    }

    void HandleIdle()
    {
        // Calculate the distance to the player
        float distanceToPlayer = Vector3.Distance(transform.position, targetedPlayer.transform.position);

        // If the player is within the chase range, start chasing the player
        if (distanceToPlayer < chaseRange)
        {
            isAttacking = true;
            // Move towards the player
            agent.SetDestination(targetedPlayer.transform.position);
            // Reset the waypoint timer
            waypointTimer = 0f;
            // Reset the attack timer
            attackTimer = 5f;
        }
        else
        {
            // Decrement the waypoint timer
            waypointTimer -= Time.deltaTime;

            if (waypointTimer <= 0)
            {
                // Calculate the distance to the current waypoint
                float distanceToWaypoint = Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position);

                // If the AI enemy is close to the waypoint, move to the next waypoint
                if (distanceToWaypoint < 1.5f)
                {
                    currentWaypointIndex++;
                    if (currentWaypointIndex >= waypoints.Length)
                    {
                        currentWaypointIndex = 0;
                    }
                    // Reset the waypoint timer
                    //waypointTimer = 1f;
                }

                // Look at the current waypoint
                //transform.LookAt(waypoints[currentWaypointIndex].position);

                // Set the destination to the current waypoint
                agent.SetDestination(waypoints[currentWaypointIndex].position);

            }
        }
    }

    void HandleAggressive()
    {
        // Turn towards the player
        transform.LookAt(targetedPlayer.transform);
        // Move towards the player
        agent.SetDestination(targetedPlayer.transform.position);
        Debug.Log("Entering Raycast");
        // Raycast from the shooting point to the player
        RaycastHit hit;

        Vector3 playerPos = targetedPlayer.transform.position;
        Vector3 raycastOrigin = transform.position;
        Vector3 direction = playerPos - raycastOrigin;

        if (Physics.Raycast(shootingPoint.position, direction, out hit, 100f, playerLayerMask))
        {
            Debug.Log("Countdown");
            // If the raycast hits the player, shoot a projectile
            shootingTimer += Time.deltaTime;
            if (shootingTimer >= shootingInterval)
            {
                Debug.Log("shoot");
                // If the Raycast hits the player, activate the muzzle flash particle system
                muzzleFlash.Play();
                // Instantiate a new projectile at the shooting point
                //GameObject projectile = Instantiate(muzzlePrefab, shootingPoint.position, shootingPoint.rotation);
                // Add force to the projectile in the direction it is facing
                //projectile.GetComponent<Rigidbody>().AddForce(shootingPoint.forward * 500f);
                // Reset the shooting timer
                shootingTimer = 0f;
                Debug.Log("Muzzleflash");
            }

        }
    }
}