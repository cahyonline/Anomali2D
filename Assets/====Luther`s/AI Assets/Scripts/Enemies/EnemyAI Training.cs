using System.Collections;
using UnityEngine;

public class EnemyAITraining : MonoBehaviour
{
////////////////////////////////////////////////////////////
/// BASIC
    public Transform player;
////////////////////////////////////////////////////////////
/// DETECTION
    [Header("Detection Settings")]
    public bool noAttack;
    public float sightRange = 5f;
    public LayerMask obstacleLayer;
    private float defaultSpeed;
    private float defaultSight;
    private float defaultBattleG;
    private float playerOnBattle;

    [Header("Shield Settings")]
    public bool shieldUp = true; // Shield starts up
    public float shieldAngle = 60f; // Angle range where shield is effective
    [Header("Turning Settings")]
    public float turnCooldown = 1f; // Cooldown before turning
    private float lastTurnTime = 0f; // Tracks last turn time
    public float attackCooldown = 3f;
    private bool turningOnCD;
    [Header("Detection Settings")]
    public float battleRange = 3f;  // New battle range
    public float attackRange = 1f;  // New attack range
    
////////////////////////////////////////////////////////////
/// PATROL SYSTEM
    [Header("Patrol Settings")]
    public Transform pointA, pointB, pointE; // Patrol points
    public Transform pointC, pointD; // Territory boundaries
    public float walkSpeed = 4f;
    private Transform currentTarget;
    private bool chasingPlayer = false;
    private bool movingToMiddle = false;
    private bool isAttacking;
////////////////////////////////////////////////////////////
/// COMPONENTS
    private Rigidbody2D rb;
    public GameObject attackHB;
    public GameObject shieldVis;
    [SerializeField] private float shieldKnockbackForce = 3f;
////////////////////////////////////////////////////////////
/// INITIALIZATION
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA; // Start by moving toward point A
        defaultSight = sightRange;
        defaultBattleG = battleRange;
        defaultSpeed = walkSpeed;
        attackHB.SetActive(false);
        //shieldVis.SetActive(false);
    }
////////////////////////////////////////////////////////////
/// UPDATE BEHAVIOR
    void FixedUpdate()
    {
        if(!IsPlayerInTerritory())
            {

                sightRange = 0f;
                battleRange =0f;
                walkSpeed = 10f;
            }
            else
            {
                sightRange = defaultSight;
                battleRange = defaultBattleG;
                walkSpeed = defaultSpeed;
            }
        if (chasingPlayer)
        {
            if (CanSeePlayer() && IsPlayerInTerritory())  
            {
                ChasePlayer();
                FacePlayer();
            }

            else if(playerOnBattle <= battleRange)
            {
                Debug.Log("Inside Battle");
            }
    
            else  
            {
                chasingPlayer = false;
    
                if (!movingToMiddle && (IsEnemyInTerritory() || IsPlayerInTerritory()))
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    Debug.Log("BACK TO MID 1");
                }
                else
                {
                    FaceAwayFromPlayer(); // Only flip when completely outside
                    StartCoroutine(MoveToMiddleAndPatrol());
                }

                if(!CanSeePlayer() && !movingToMiddle && (IsEnemyInTerritory() || IsPlayerInTerritory()))
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    Debug.Log("BACK TO MID 2");
                }
            }
        }
        else if(noAttack)
        {
            if(shieldUp)
            {
                shieldUp = true;
                shieldVis.SetActive(true);
            }
            if(!shieldUp)
            {
                shieldUp = false;
                shieldVis.SetActive(false);
            }           
            return;
        }
        else
        {
            Patrol(); // Resume patrolling
            if (CanSeePlayer() && IsPlayerInTerritory()) 
            {
                chasingPlayer = true; // Start chasing player
            }
        }
        
            if(shieldUp == true)
            {
                shieldVis.SetActive(true);
            }
            else
            {
                shieldVis.SetActive(false);
            }
    }
////////////////////////////////////////////////////////////
/// DETECTION SYSTEM (RAYCAST)
    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer > sightRange) 
        return false;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, obstacleLayer | (1 << player.gameObject.layer));

        Debug.DrawRay(transform.position, directionToPlayer.normalized * distanceToPlayer, Color.cyan);

        return hit.collider != null && hit.collider.gameObject == player.gameObject;
    }

    bool IsPlayerInTerritory()
    {
        return player.position.x >= pointC.position.x && player.position.x <= pointD.position.x;
    }

    bool IsEnemyInTerritory()
    {
        return transform.position.x >= pointC.position.x && transform.position.x <= pointD.position.x;
    }

    bool IsPlayerInPatrolZone()
    {
        return player.position.x >= pointA.position.x && player.position.x <= pointB.position.x;
    }

    bool IsEnemyInPatrolZone()
    {
        return transform.position.x >= pointA.position.x && transform.position.x <= pointB.position.x;
    }
////////////////////////////////////////////////////////////
/// MOVEMENT SYSTEMS
IEnumerator MoveToMiddleAndPatrol()
{
    movingToMiddle = true;

    // Face the middle before moving
    FaceDirection(pointE.position);

    // Move to middle point E first
    currentTarget = pointE;
    yield return new WaitUntil(() => Vector2.Distance(transform.position, pointE.position) < 0.1f);

    // After reaching point E, randomly pick A or B
    currentTarget = Random.value > 0.5f ? pointA : pointB;

    // Face the new patrol target
    FaceDirection(currentTarget.position);

    movingToMiddle = false;
}

void ChasePlayer()
{
    //FaceDirection(player.position);
    if (isAttacking) return; // Stop movement while attacking
    if (!turningOnCD) return;

    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    if (distanceToPlayer <= attackRange)
    {
        FaceDirection(player.position);
        StartCoroutine(Attack()); // Start attack when in range
        return;
    }

    // Move towards the player if not attacking
    Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
    transform.position = Vector2.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
    playerOnBattle = distanceToPlayer;
    if (distanceToPlayer <= battleRange && Time.time >= lastTurnTime + turnCooldown)
    {
        FaceDirection(player.position);
        lastTurnTime = Time.time;
        Debug.Log("This Thing");
    }
}

void FaceAwayFromPlayer()
{
    if (!IsEnemyInPatrolZone() && !IsPlayerInPatrolZone())
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }
}

void Patrol()
{
    Vector2 noYmovement = new Vector2(currentTarget.position.x, transform.position.y);
    transform.position = Vector2.MoveTowards(transform.position, noYmovement, walkSpeed * Time.deltaTime);
    
    if(Vector2.Distance(transform.position, noYmovement) < 0.1f)
    {
        currentTarget = currentTarget == pointA ? pointB : pointA;
        FaceDirection(currentTarget.position);
    }
}

void FaceDirection(Vector2 target)
{
    Vector3 currentScale = transform.localScale;
    currentScale.x = (target.x > transform.position.x) ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
    transform.localScale = currentScale;
}

public bool IsShieldBlocking(Vector2 attackPosition)
{
    if (!shieldUp) return false; // If shield is down, allow attack

    // Calculate the direction of the attack relative to the enemy
    Vector2 attackDirection = (attackPosition - (Vector2)transform.position).normalized;
    Vector2 enemyDirection = transform.right * (transform.localScale.x > 0 ? 1 : -1); // Facing direction

    float angle = Vector2.Angle(enemyDirection, attackDirection);

    // Add a minimum attack distance check to prevent close-range issues
    float minAttackDistance = 0.3f; // Adjust as needed
    float attackDistance = Vector2.Distance(transform.position, attackPosition);

    return angle < shieldAngle / 2f && attackDistance > minAttackDistance;
}
void FacePlayer()
{

        if (Time.time < lastTurnTime + turnCooldown)
        {
            turningOnCD = true;
            return;

        } // Wait for cooldown

    Vector3 currentScale = transform.localScale;
    if (player.position.x > transform.position.x)
    {
        currentScale.x = Mathf.Abs(currentScale.x); // Face right
    }
    else
    {
        currentScale.x = -Mathf.Abs(currentScale.x); // Face left
    }
    transform.localScale = currentScale;

    lastTurnTime = Time.time; // Reset cooldown
    turningOnCD = false;
}
IEnumerator Attack()
{
    if (isAttacking) yield break; // Prevent multiple attack calls

    isAttacking = true; // Mark that the enemy is attacking
    shieldUp = false; // Lower shield
    attackHB.SetActive(true); // Enable attack hitbox

    yield return new WaitForSeconds(0.5f); // Attack duration

    attackHB.SetActive(false); // Disable attack hitbox
    shieldUp = true; // Raise shield again

    yield return new WaitForSeconds(attackCooldown); // Cooldown before attacking again

    isAttacking = false; // Allow new attacks
}
private void Knockback(Vector2 attackDirection)
{
    rb.velocity = attackDirection * shieldKnockbackForce;
}

void OnTriggerStay2D(Collider2D other)
{
    if (other.CompareTag("PlayerDamage"))
    {
        Vector2 attackDirection = (transform.position - other.transform.position).normalized;

        if (IsShieldBlocking(other.transform.position))
        {
            Knockback(attackDirection);
            Debug.Log("Damage Blocked");
            return; // Block attack, do nothing
        }
        
        //TakeDamage(); // If not blocked, take damage
        Debug.Log("Damage Taken");
    }
}
////////////////////////////////////////////////////////////
/// GIZMOS FOR DEBUGGING
void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, sightRange);

    Gizmos.color = Color.blue;
    Gizmos.DrawWireSphere(transform.position, battleRange); // Battle range

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange); // Attack range

    //Gizmos.color = Color.black;
    //Gizmos.DrawWireSphere(transform.position, shieldAngle);
}
}