using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAIMelee : MonoBehaviour
{
////////////////////////////////////////////////////////////
/// BASIC
    public Transform player;
////////////////////////////////////////////////////////////
/// DETECTION
    [Header("Detection Settings")]
    private bool vulnerable;
    public float sightRange = 5f;
    public float battleRange = 5f;
    public LayerMask obstacleLayer;
    private float defaultSpeed;
    private float defaultSight;
    private float defaultBattleG;
    private bool insideBattleRange;

    [Header("Shield Settings")]
    public float stunnedDuration = 2f;
    public bool shieldUp = true; // Shield starts up
    public float shieldAngle = 60f; // Angle range where shield is effective
    private bool isStunned = false;

    [Header("Turning Settings")]
    public float reactionTime = 3f;
    private float defaultReactionTime;
    public float attackCooldown = 3f;
    private bool isReadyReaction = true;

    [Header("Attack Settings")]
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
/// HEALTH
    [Header("HEALTH")]
    public Image healthBar;
    public float healthAmount = 200f;
    public float PlayerDamage = 35f;
    private float Whathit = 0.5f;

    [Header("IMPORTANT")]
    public GameObject EnemyRootObject;
    public Collider2D ThisEnemyCollider2D;
    
////////////////////////////////////////////////////////////
/// COMPONENTS
    private Rigidbody2D rb;
    public GameObject attackHB;
    public GameObject shieldVis;
    public static bool isBlocked;
    private bool lastPlayerLocation;
    private bool isDead = false;
    public Animator meleeAN;
    private float playerPosition;
    [SerializeField] private float shieldKnockbackForce = 3f;
////////////////////////////////////////////////////////////
/// INITIALIZATION
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA; 
        defaultSight = sightRange;
        defaultSpeed = walkSpeed;
        defaultBattleG = battleRange;
        defaultReactionTime = reactionTime;
        attackHB.SetActive(false);
        //shieldVis.SetActive(false);

    }
////////////////////////////////////////////////////////////
/// UPDATE BEHAVIOR
    void FixedUpdate()
    {
        if(!IsPlayerInTerritory() && !isDead)
            {

                sightRange = 0f;
                walkSpeed = 10f;
                battleRange = 0f;
            }
            else
            {
                sightRange = defaultSight;
                battleRange = defaultBattleG;
                walkSpeed = defaultSpeed;
            }
        if (chasingPlayer)
        {
            if (CanSeePlayer() && IsPlayerInTerritory() && !isDead)  
            {
                ChasePlayer();
                //FacePlayer();
            }
    
            else  
            {
                chasingPlayer = false;
    
                if (!movingToMiddle && (IsEnemyInTerritory() || IsPlayerInTerritory()) && !isDead)
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    Debug.Log("BACK TO MID 1");
                }
                else
                {
                    FaceAwayFromPlayer(); // Only flip when completely outside
                    StartCoroutine(MoveToMiddleAndPatrol());
                }

                if(!CanSeePlayer() && !movingToMiddle && (IsEnemyInTerritory() || IsPlayerInTerritory()) && !isDead)
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    Debug.Log("BACK TO MID 2");
                }
            }
        }
        else
        {
            if(!isDead)
            {
               Patrol(); // Resume patrolling 
            }
            
            if (CanSeePlayer() && IsPlayerInTerritory() && !isDead) 
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

        if(healthAmount <=0)
        {
            isDead = true;
            Dies();
        }

        if (playerPosition < battleRange)
        {
            insideBattleRange = true;
        }
        if (playerPosition > sightRange)
        {
            Debug.Log("outside");
            //reactionTime = 0f;
            isAttacking = false;
            //meleeAN.SetBool("turnAN",false);
        }
        
    }
////////////////////////////////////////////////////////////
/// DETECTION SYSTEM (RAYCAST)
    bool CanSeePlayer()
    {
        Vector2 directionToPlayer = player.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;
        playerPosition = distanceToPlayer;

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
    //if (!turningOnCD) return;
    Debug.Log("Chase");
    //meleeAN.SetBool("turnAN",false);
    meleeAN.SetBool("walkAN",true);
    
        StartCoroutine(FacePlayer());


    float distanceToPlayer = Vector2.Distance(transform.position, player.position);

    if (distanceToPlayer <= attackRange && !isStunned && !isDead)
    {
        FaceDirection(player.position);
        StartCoroutine(Attack()); // Start attack when in range
        return;
    }

    if (isStunned)
    {
        Debug.LogWarning("STUNED");
        meleeAN.SetBool("walkAN",false);
        return;
    }

    // Move towards the player if not attacking
    Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
    transform.position = Vector2.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);
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
    meleeAN.SetBool("walkAN",true);
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
IEnumerator FacePlayer()
{
 // Wait for cooldown
    if(insideBattleRange)
    {

        if(!isStunned || isReadyReaction)
            {
            StartCoroutine(FacePlayerCD());
        }
    
        yield return new WaitForSeconds(0);
        Debug.LogWarning("FacePlayer done");
    }
    else if(!isStunned)
    {
        FaceDirection(player.position);
    }
    
}

IEnumerator FacePlayerCD()
{
    isReadyReaction = false;
    yield return new WaitForSeconds(reactionTime);
    FaceDirection(player.position);
    Debug.Log("Reaction reset");
    isReadyReaction = true;
}
IEnumerator Attack()
{
    //meleeAN.SetBool("turnAN",false);
    //reactionTime = defaultReactionTime;
    meleeAN.SetBool("walkAN",false);
    meleeAN.SetBool("attAN",true);
    if (isAttacking) yield break; // Prevent multiple attack calls
    if (isDead) {isAttacking = false;yield break;} 

    isAttacking = true; // Mark that the enemy is attacking
    shieldUp = false; // Lower shield
    attackHB.SetActive(true); // Enable attack hitbox

    yield return new WaitForSeconds(0.2f); // Attack duration

    meleeAN.SetBool("attAN",false);
    
    attackHB.SetActive(false); // Disable attack hitbox
    shieldUp = true; // Raise shield again

    if(isDead ||playerPosition > sightRange) {isAttacking = false;yield break;}

    yield return new WaitForSeconds(attackCooldown); // Cooldown before attacking again

    meleeAN.SetBool("walkAN",false);
    if (isDead || playerPosition > sightRange) {isAttacking = false;yield break;}
    bool playerOnTheRight = player.position.x > transform.position.x;
    if (playerOnTheRight != lastPlayerLocation && isAttacking)
    {
        Vector3 currentlyScale = transform.localScale;
        currentlyScale.x = playerOnTheRight ? Mathf.Abs(currentlyScale.x) : -Mathf.Abs(currentlyScale.x);
        transform.localScale = currentlyScale;
        meleeAN.SetBool("turnAN",true);
        lastPlayerLocation = playerOnTheRight;
    }
    
    Debug.Log("Face Player");

        Vector3 currentScale = transform.localScale;
    if (player.position.x > transform.position.x)
    {
        currentScale.x = Mathf.Abs(currentScale.x); // Face right
    }

    transform.localScale = currentScale;

    yield return new WaitForSeconds(1f);

    if (isDead || playerPosition > sightRange) {yield break;}
    isAttacking = false;
    meleeAN.SetBool("turnAN",false);

   
}
private void Knockback(Vector2 attackDirection)
{
    rb.velocity = attackDirection * shieldKnockbackForce;
    meleeAN.SetBool("knockAN",true);
    meleeAN.SetBool("walkAN",false);
    isStunned = true;
    StartCoroutine(AttackReset());
    StartCoroutine(AnimationReset());
}

IEnumerator AttackReset()
{
    yield return new WaitForSeconds(stunnedDuration);
    isStunned = false;
}
////////////////////////////////////////////////////////////
/// HEALTH DAMAGE COUNTER
void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("PlayerDamage") && !isDead )
    {
        Vector2 attackDirection = (transform.position - other.transform.position).normalized;

        if (other.CompareTag("PlayerDamage") && IsShieldBlocking(other.transform.position))
        {
            Knockback(attackDirection);
            Debug.Log("Damage Blocked");
            isBlocked = true;
            //vulnerable = false;
            //StartCoroutine(InvulnerableCD());
            return; // Block attack, do nothing
        }
        
        //TakeDamage(); // If not blocked, take damage
        meleeAN.SetBool("hurtAN",true);
        //Debug.Log("Damage Taken");
        StartCoroutine(AnimationReset());
        TakeDamage(PlayerDamage);
        isBlocked = false;
    }
}

IEnumerator AnimationReset()
{
    yield return new WaitForSeconds(0.5f);
    {
        meleeAN.SetBool("hurtAN",false);
        meleeAN.SetBool("knockAN",false);
    }
}
////////////////////////////////////////////////////////////
/// HEALTH DAMAGE COUNTER
        public void TakeDamage (float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / 200f;
    }
    public void Healing (float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, 200);
        healthBar.fillAmount = healthAmount / 200f;
    }

    void Dies()
    {
        //isDead = true;
        shieldUp = false;
        shieldVis.SetActive(false);
        meleeAN.SetBool("deadAN",true);
        meleeAN.SetBool("walkAN", false);
        meleeAN.SetBool("turnAN",false);
        attackHB.SetActive(false);
            sightRange = 0f;
            walkSpeed = 0f;
            battleRange = 0f;
            rb.bodyType = RigidbodyType2D.Static;
            ThisEnemyCollider2D.enabled = false;
            StartCoroutine(DespawnGameobject());
    }

    IEnumerator DespawnGameobject()
    {
        yield return new WaitForSeconds(5);
        EnemyRootObject.SetActive(false);
    }
///////////////////////////////////////////////////////////////////
////// DAMAGE COOLDOWN
    //private IEnumerator InvulnerableCD()
    //{
        //yield return new WaitForSeconds(Whathit);
        //vulnerable = true;
        //Debug.LogWarning("vulnerable");
    //}
////////////////////////////////////////////////////////////
/// GIZMOS FOR DEBUGGING
void OnDrawGizmos()
{
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, sightRange);

    Gizmos.color = Color.red;
    Gizmos.DrawWireSphere(transform.position, attackRange); // Attack range

    //Gizmos.color = Color.blue;
    //Gizmos.DrawWireSphere(transform.position, battleRange);
    //Gizmos.color = Color.black;
    //Gizmos.DrawWireSphere(transform.position, shieldAngle);
}
}