using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAIGBoss : MonoBehaviour
{
////////////////////////////////////////////////////////////
/// BASIC
    public Transform player;
    [Header("Sprite Setting")]
    public bool lookLeft; 

////////////////////////////////////////////////////////////
/// DETECTION
    [Header("Detection Settings")]
    public float sightRange = 5f;
    public LayerMask obstacleLayer;
    private float defaultSpeed;
    private float defaultSight;
    private float defaultAttackRange;

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
    public float healthAmount;
    public float PlayerDamage = 35f;
    private bool vulnerable;
    private float Whathit = 0.5f;

    [Header("IMPORTANT")]
    //public GameObject EnemyRootObject;
    public Collider2D ThisEnemyCollider;
    
////////////////////////////////////////////////////////////
/// COMPONENTS
    private Rigidbody2D rb;
    public GameObject attackHB;
    public Animator bossAnimator;
    private bool hurtCanceled;
    private float distanceToPlayer;
    private bool isDead;
    //private bool inMiddle;
////////////////////////////////////////////////////////////
/// INITIALIZATION
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentTarget = pointA; 
        defaultSight = sightRange;
        defaultAttackRange = attackRange;
        healthAmount = MaxHealth;
        sightRange = 0f;
        attackRange = 0f;
        //defaultSpeed = walkSpeed;
        FlipSprite();
        attackHB.SetActive(false);
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
                //sightRange = defaultSight;
            }
        if (chasingPlayer)
        {
            if (CanSeePlayer() && IsPlayerInsideTerritory())  
            {
                ChasePlayer();
                FacePlayer();
            }
            else  
            {
                chasingPlayer = false;

                if (!movingToMiddle)
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    //Debug.Log("BACK TO MID 1");
                }
                else
                {
                    //FaceAwayFromPlayer(); // Only flip when completely outside
                    StartCoroutine(MoveToMiddleAndPatrol());
                }

                if(!CanSeePlayer() && !movingToMiddle)
                {
                    StartCoroutine(MoveToMiddleAndPatrol());
                    //Debug.Log("BACK TO MID 2");
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
            //Debug.LogError("Enemy DIED");
            isDead = true;
            MEMEK();
            AudioManager.Instance.PlayMusic("BgmV10");
            //AudioManager.Instance.SFXaddOn("DiesBosses");
            Dies();
            //Destroy(EnemyRootObject);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            StartBossFight();
            Debug.LogWarning("P pressed");
        }

        distanceToPlayer = Vector2.Distance(transform.position, player.position);
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
        //inMiddle = false;
        bossAnimator.SetBool("runAN",false);


        // Face the middle before moving
        //FaceDirection(pointE.position);

        // Move to middle point E first
        currentTarget = pointE;
        yield return new WaitUntil(() => Vector2.Distance(transform.position, pointE.position) < 0.1f);

        // After reaching point E, randomly pick A or B
        currentTarget = Random.value > 0.5f ? pointA : pointB;
        //inMiddle = true;

        // Face the new patrol target
        //FaceDirection(currentTarget.position);
        movingToMiddle = false;
    }

    void ChasePlayer()
    {
        if (isAttacking) return; // Stop movement while attacking
        //inMiddle = false;
        //FacePlayer();

        //float distanceToPlayer = Vector2.Distance(transform.position, player.position);

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

        bossAnimator.SetBool("runAN",true);
        bossAnimator.SetBool("attAN",false);
    }
    void Patrol()
    {
        if (Vector2.Distance(transform.position, pointE.position) > 1f)
        {
            Vector2 noYmovement = new Vector2(pointE.position.x, transform.position.y);
            transform.position = Vector2.MoveTowards(transform.position, noYmovement, walkSpeed * Time.deltaTime);
            //FaceDirection(pointE.position);
            //Debug.LogError("This bit1");

        }

        else
        {
            Idle();
            rb.velocity = Vector2.zero;
            //Debug.LogError("This bit3");
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

    void Idle()
    {
        rb.velocity = Vector2.zero;
        bossAnimator.SetBool("runAN",false);
        bossAnimator.SetBool("attAN",false);

        //Debug.LogWarning("IDLE");
    }
    // void FaceDirection(Vector2 target)
    // {
    //     Vector3 currentScale = transform.localScale;
    //     currentScale.x = (target.x > transform.position.x) ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
    //     transform.localScale = currentScale;
    // }

    void FacePlayer()
    {
        Vector3 currentScale = transform.localScale;
        currentScale.x = (player.position.x > transform.position.x) ? Mathf.Abs(currentScale.x) : -Mathf.Abs(currentScale.x);
        transform.localScale = currentScale;
        FlipSprite();
        lastTurnTime = Time.time; // Reset cooldown
    }

    public void StartBossFight()
    {
        sightRange = defaultSight;
        attackRange = defaultAttackRange;
    }


    IEnumerator Attack()
    {
        if(!isDead)
        {
            if (isAttacking) yield break; // Prevent multiple attack calls
            hurtCanceled = true;

            bossAnimator.SetBool("runAN",false);
            isAttacking = true;

            bossAnimator.SetBool("attAN",true);
            AudioManager.Instance.PlaySFX("AttackBoss");


            yield return new WaitForSeconds(0.4f);

            hurtCanceled = false;
            bossAnimator.SetBool("attAN",false);
            attackHB.SetActive(true);

            yield return new WaitForSeconds(0.4f);
            attackHB.SetActive(false);       

            yield return new WaitForSeconds(attackCooldown);

            bossAnimator.SetBool("attAN",false);
            isAttacking = false;            
        }

        else
        {
            yield break;
        }

    }


    ////////////////////////////////////////////////////////////
    /// HEALTH DAMAGE COUNTER
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("PlayerDamage"))
        {
            StartCoroutine(InvulnerableCD());
            TakeDamage(PlayerDamage);


            if (!hurtCanceled)
            {
                EventCallBack.HitStop.Invoke();
                //Debug.Log("Enemy Took Damage");
                StartCoroutine(AnimationReset());
            }
        }
        
        if (other.CompareTag("PlayerDamageBig"))
        {
            TakeDamage(PlayerDamage * 3);

            if (!hurtCanceled)
            {
                EventCallBack.HitStop.Invoke();
                //Debug.Log("Enemy Took Damage");
                StartCoroutine(AnimationReset());
            }
        }

    }
    
    void Dies()
    {
        //bossAnimator.SetBool("deadAN",true);
        bossAnimator.SetBool("attAN",false);
        bossAnimator.SetBool("runAN",false);

        

        sightRange = 0f;
        walkSpeed = 0f;
        rb.bodyType = RigidbodyType2D.Static;
        ThisEnemyCollider.enabled = false;
        
        //StartCoroutine(DespawnGameobject());

    }

    IEnumerator DespawnGameobject()
    {
        yield return new WaitForSeconds(10f);
        //EnemyRootObject.SetActive(false);
    }
    IEnumerator AnimationReset()
    {
        if(hurtCanceled) yield break;
        yield return new WaitForSeconds(0.2f);
        bossAnimator.SetBool("hurtAN",false);
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
    //
////////////////////////////////////////////////////////////
/// GIZMOS FOR DEBUGGING
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
//////////////
///
    private  void MEMEK()
    {
        AudioManager.Instance.SFXaddOn("DiesBosses");
    }
}

