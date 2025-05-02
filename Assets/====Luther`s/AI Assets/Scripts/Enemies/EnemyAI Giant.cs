using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAIGaint : MonoBehaviour
{
////////////////////////////////////////////////////////////
/// BASIC
    public Transform player;
    [Header("Sprite Setting")]
    public bool spriteFlip = false; 

////////////////////////////////////////////////////////////
/// DETECTION
    [Header("Detection Settings")]
    public float sightRange = 5f;
    public LayerMask obstacleLayer;
    private float defaultSpeed;
    private float defaultSight;

    [Header("Turning Settings")]
    public float turnCooldown = 1f; // Cooldown before turning
    private float lastTurnTime = 0f; // Tracks last turn time
    public float attackCooldown = 3f;

    [Header("Attack Settings")]
    public float attackRange = 1f;
    
////////////////////////////////////////////////////////////
/// PATROL SYSTEM
    [Header("Patrol Settings")]
    public Transform pointA, pointB, pointE; // Patrol points
    public float walkSpeed = 4f;
    private Transform currentTarget;
    private bool chasingPlayer = false;
    private bool movingToMiddle = false;
    private bool isAttacking;
////////////////////////////////////////////////////////////
/// HEALTH
    [Header("HEALTH")]
    public Image healthBar;
    public float MaxHealth = 200f;
    private float healthAmount;
    public float PlayerDamage = 35f;
    private bool vulnerable;
    private float Whathit = 0.5f;

    [Header("IMPORTANT")]
    public GameObject EnemyRootObject;
    
////////////////////////////////////////////////////////////
/// COMPONENTS
    private Rigidbody2D rb;
    public GameObject attackHB;
////////////////////////////////////////////////////////////
/// INITIALIZATION
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA; 
        defaultSight = sightRange;
        healthAmount = MaxHealth;
        //defaultSpeed = walkSpeed;
        attackHB.SetActive(false);

        if(spriteFlip)
        {
            FlipSprite();
        }
    }
////////////////////////////////////////////////////////////
/// UPDATE BEHAVIOR
    void FixedUpdate()
    {
        if(!IsPlayerInsideTerritory())
            {
                sightRange = 0f;
            }
            else
            {
                sightRange = defaultSight;
            }
        if (chasingPlayer)
        {
            if (CanSeePlayer() && IsPlayerInsideTerritory())  
            {
                ChasePlayer();
                //FacePlayer();
            }
            else  
            {
                chasingPlayer = false;

                if (!movingToMiddle)
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    Debug.Log("BACK TO MID 1");
                }
                else
                {
                    //FaceAwayFromPlayer(); // Only flip when completely outside
                    StartCoroutine(MoveToMiddleAndPatrol());
                }

                if(!CanSeePlayer() && !movingToMiddle)
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    Debug.Log("BACK TO MID 2");
                }
            }
        }
        else
        {
            Patrol(); // Resume patrolling
            if (CanSeePlayer()) 
            {
                chasingPlayer = true; // Start chasing player
            }
        }

        if(healthAmount <=0)
        {
            Debug.LogError("Enemy DIED");
            Destroy(EnemyRootObject);
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
////////////////////////////////////////////////////////////
/// MOVEMENT SYSTEMS
    IEnumerator MoveToMiddleAndPatrol()
    {
        movingToMiddle = true;

        // Face the middle before moving
        //FaceDirection(pointE.position);

        // Move to middle point E first
        currentTarget = pointE;
        yield return new WaitUntil(() => Vector2.Distance(transform.position, pointE.position) < 0.1f);

        // After reaching point E, randomly pick A or B
        currentTarget = Random.value > 0.5f ? pointA : pointB;

        // Face the new patrol target
        //FaceDirection(currentTarget.position);

        movingToMiddle = false;
    }

    void ChasePlayer()
    {
        if (isAttacking) return; // Stop movement while attacking
        //FacePlayer();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            //FaceDirection(player.position);
            StartCoroutine(Attack()); // Start attack when in range
            return;
        }

        // Move towards the player if not attacking
        Vector2 targetPos = new Vector2(player.position.x, transform.position.y);
        transform.position = Vector2.MoveTowards(transform.position, targetPos, walkSpeed * Time.deltaTime);

        if (Time.time >= lastTurnTime + turnCooldown)
        {
            //FaceDirection(player.position);
            lastTurnTime = Time.time;
        }
    }

    void Patrol()
    {
        if (Vector2.Distance(transform.position, pointE.position) > 0.1f)
        {
            Vector2 noYmovement = new Vector2(pointE.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, noYmovement, walkSpeed * Time.deltaTime);
            //FaceDirection(pointE.position);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
    bool IsPlayerInsideTerritory()
    {
        float minX = Mathf.Min(pointA.position.x, pointB.position.x);
        float maxX = Mathf.Max(pointA.position.x, pointB.position.x);

        return player.position.x >= minX && player.position.x <= maxX;
    }
    void FlipSprite()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x *= -1;
        transform.localScale = currentScale;
    }

    // void FaceDirection(Vector2 target)
    // {
    //     Vector3 currentScale = transform.localScale;
    //     currentScale.x = (target.x > transform.position.x) ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
    //     transform.localScale = currentScale;
    // }

    // void FacePlayer()
    // {
    //     Vector3 currentScale = transform.localScale;
    //     currentScale.x = (player.position.x > transform.position.x) ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
    //     transform.localScale = currentScale;

    //     lastTurnTime = Time.time; // Reset cooldown
    // }

    IEnumerator Attack()
    {
        if (isAttacking) yield break; // Prevent multiple attack calls

        isAttacking = true;
        attackHB.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        attackHB.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }


////////////////////////////////////////////////////////////
/// HEALTH DAMAGE COUNTER
    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("PlayerDamage"))
        {
            StartCoroutine(InvulnerableCD());
            TakeDamage(PlayerDamage);
            //Debug.Log("Enemy Took Damage");
        }

    }

    public void TakeDamage(float damage)
    {
        healthAmount -= damage;
        healthBar.fillAmount = healthAmount / MaxHealth;
    }

    public void Healing(float healAmount)
    {
        healthAmount += healAmount;
        healthAmount = Mathf.Clamp(healthAmount, 0, MaxHealth);
        healthBar.fillAmount = healthAmount / MaxHealth;
    }

    private IEnumerator InvulnerableCD()
    {
        yield return new WaitForSeconds(Whathit);
    }

////////////////////////////////////////////////////////////
/// GIZMOS FOR DEBUGGING
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}