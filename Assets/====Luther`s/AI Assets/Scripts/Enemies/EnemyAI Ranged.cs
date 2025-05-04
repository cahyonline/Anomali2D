using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class EnemyAIRanged : MonoBehaviour
{
////////////////////////////////////////////////////////////
/// BASIC
    public Transform player;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public Animator enemyANIM;
////////////////////////////////////////////////////////////
/// RAW DETECT PLAYER
    [Header("Detection Settings")]
    public float sightRange = 5f;
    public float fleeRange = 2f;
////////////////////////////////////////////////////////////
/// MOVEMENT N SHOOT
    [Header("Movement Settings")]
    public float fleeSpeed = 3f;
    private float defaultFleeR;
    public float fireCD = 2f;
    public float FleeCooldown =4f;
    private float nextFireTime = 1f;
    private bool canFlee = true;
    private bool isFleeing = false;
    private bool isForceFight = false;
    //private float fleeCD = 0f;
    //private bool EmergencyCDCD =false;
////////////////////////////////////////////////////////////
/// RAW WALL DETECT RULE
    [Header("Wall Detection")]
    public LayerMask wallLayer;
    public float wallCheckDistance = 0.5f;
    private Rigidbody2D rb;
////////////////////////////////////////////////////////////
/// PLAYER DETECT ADVANCED
    // Line of Sight detection variables
    public LayerMask obstacleLayer;
    public float fieldOfViewAngle = 360f;
    private float defaultFieldOfViewAnge;
    private float defaultSight;
////////////////////////////////////////////////////////////
/// HEALTH 
    [Header("HEALTH")]
    public UnityEngine.UI.Image healthBar;
    public float healthAmount = 200f;
    private float Whathit = 2f;
    public float PlayerDamage = 35f;
    public bool vulnerable = true;
    private bool isDied = false;

    private bool takingDamage = false;
    //[Header("IMPORTANT")]
    //public GameObject EnemyRootObject;

////////////////////////////////////////////////////////////
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultFleeR = fleeRange;
        defaultSight = sightRange;
        defaultFieldOfViewAnge = fieldOfViewAngle;
    }
////////////////////////////////////////////////////////////
    void FixedUpdate()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (!isDied && distanceToPlayer <= fleeRange && canFlee && !takingDamage )
        {
            StartCoroutine(Flee());
        }
        else if (!isDied && distanceToPlayer <= sightRange && !isFleeing && CanSeePlayer() && !takingDamage)
        {
            //Debug.LogWarning("PLayer in range and shopoting");
            FacePlayer();
            ShootProjectile();
            //playerANIM.SetBool("AttANIM" ,true);
        }
        else if (!isDied && distanceToPlayer <= sightRange && CanSeePlayer() && isForceFight && !takingDamage)
        {
            FacePlayer();
            ShootProjectile();
        }

        else
        {
            Idle();
            enemyANIM.SetBool("attAN" ,false);
            enemyANIM.SetBool("runAN" ,false);
            
        }

        if(isDied)
        {
            Idle();
        }
        
        if(healthAmount <=0)
        {
            //Debug.LogError("Enemy DIED");
            //Destroy(EnemyRootObject);
            enemyANIM.SetBool("dieAN" ,true);
            enemyANIM.SetBool("attAN" ,false);
            enemyANIM.SetBool("runAN" ,false);
            isDied = true;
            
        }

        if (takingDamage)
        {
            TakesDamagesFromPlayer();
            enemyANIM.SetBool("attAN" ,false);
            enemyANIM.SetBool("runAN" ,false);
            //Idle();
        }
        
    }
////////////////////////////////////////////////////////////
/// ADVANCED PLAYER DETECT
bool CanSeePlayer()
{
    Vector2 directionToPlayer = player.position - transform.position;
    float distanceToPlayer = directionToPlayer.magnitude;

    // Limit the sight range to a maximum of 10f or the actual distance to the player (whichever is smaller)
    float maxRayDistance = Mathf.Min(distanceToPlayer, sightRange);

    // Angle check
    float angle = Vector2.Angle(transform.right, directionToPlayer);
    if (angle < fieldOfViewAngle)
    {
        // Raycast with a limited distance
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer.normalized, maxRayDistance, obstacleLayer | (1 << player.gameObject.layer));

        // Draw the ray in debug mode
        Debug.DrawRay(transform.position, directionToPlayer.normalized * maxRayDistance, Color.magenta);

        // Return true only if the ray hits the player
        if (hit.collider != null && hit.collider.gameObject == player.gameObject)
        {
            return true;
        }
    }
    return false;
}
////////////////////////////////////////////////////////////
/// PROJECTILE
    void ShootProjectile()
    {
        //playerANIM.SetBool("IdleANIM" ,false);
        if (Time.time >= nextFireTime && !takingDamage)
        {
            nextFireTime = Time.time + fireCD;

            enemyANIM.SetBool("attAN" ,true);
            StartCoroutine(DelayBeforeShoot());
            StartCoroutine(AnimatorCD());
            
            //playerANIM.SetBool("cooldownAN",true);
            //StartCoroutine(AnimatorCD());
        }
        //playerANIM.SetBool("AttkANIM" ,false);
    }

    IEnumerator AnimatorCD()
    {
        yield return new WaitForSeconds(0.5f);
        //playerANIM.SetBool("cooldownAN",false);
        //playerANIM.SetBool("attAN" ,true);
        enemyANIM.SetBool("attAN",false);
    }
    IEnumerator DelayBeforeShoot()
    {
        yield return new WaitForSeconds(0.3f);
        GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            
         // Determine the direction based on enemy's local scale
        int direction = transform.localScale.x < 0 ? -1 : 1;
            
        // SEND DIRECTION TO PP
        newProjectile.GetComponent<ProjectileProperties>().SetDirection(direction);
        //playerANIM.SetBool("attAN",false);
    }
////////////////////////////////////////////////////////////
/// HEALTH DAMAGE COUNTER
void OnTriggerEnter2D(Collider2D other)
{
    if (other.CompareTag("PlayerDamage") && !isDied)
    {
        takingDamage = true;
    }
}

public void TakesDamagesFromPlayer()
{
    //TakeDamage(); // If not blocked, take damage
    fieldOfViewAngle = 0f;
    fleeRange = 0f;
    sightRange = 0f;
    canFlee = false;
    Debug.Log("Damage Taken");

    TakeDamage(PlayerDamage);
    enemyANIM.SetBool("hurtAN", true);

    StartCoroutine(InvulnerableCD());
    StartCoroutine(AnimatorHitCD());
    //takingDamage = false;
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
////////////////////////////////////////////////////////////
/// FLEE RANDOMIZED
IEnumerator Flee()
{
    fieldOfViewAngle = 0f;
    sightRange = 0f;
    enemyANIM.SetBool("runAN" ,true);
    fleeRange = 0f;
    canFlee = false;
    isFleeing = true;
    float defaultFleeCD = 0.2f; //Flee range dissapear CD

    Vector2 fleeDirection;

    // 50 50
    if (Random.value < 0.5f) 
    {
        fleeDirection = (transform.position - player.position).normalized; // Flee away
        fleeDirection.y = 0;
        Idle();
    }
    else
    {
        fleeDirection = (player.position - transform.position).normalized; // Flee toward
        fleeDirection.y = 0;
        Idle();
    }

    // If the flee direction is too small, default to a random direction
    if (fleeDirection.magnitude < 0.1f)
    {
        fleeDirection = new Vector2(1f, 0f);
        Idle();
    }

    // Check if there's a wall in the flee direction
    RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, fleeDirection, wallCheckDistance, wallLayer);
    
    if (wallCheck.collider != null)
    {
        // If a wall blocks fleeing away, force the enemy to flee toward the player instead
        fleeDirection = (player.position - transform.position).normalized;
        Idle();
    }

    // Move enemy in flee direction
    FaceAwayFromPlayer(fleeDirection);
    Vector2 targetPosition = (Vector2)transform.position + fleeDirection * fleeSpeed * 1f;

    while ((Vector2)transform.position != targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, fleeSpeed * Time.deltaTime);
        
        wallCheck = Physics2D.Raycast(transform.position, fleeDirection, wallCheckDistance, wallLayer);
        if (wallCheck.collider != null)
        {
            break;
        }
        yield return null; // Wait 4 frame
    }

    //Debug.Log("Fleeing complete, returning to normal behavior.");

    yield return new WaitForSeconds(defaultFleeCD);
    isFleeing = false;
    canFlee = true;
    fleeRange = defaultFleeR;
    enemyANIM.SetBool("RunANIM" ,false);
    StartCoroutine(FleeChanceCoolDown());
}
////////////////////////////////////////////////////////////
    void Idle()
    {
        rb.velocity = Vector2.zero;
        fieldOfViewAngle = defaultFieldOfViewAnge;
        sightRange = defaultSight;
        //fleeRange = defaultFleeR;
        //playerANIM.SetBool("IdleANIM" ,true);
    }

    void FacePlayer()
    {
        Vector3 currentScale = transform.localScale;
        if (player.position.x > transform.position.x)
        {
            currentScale.x = Mathf.Abs(currentScale.x); // right
        }
        else
        {
            currentScale.x = -Mathf.Abs(currentScale.x); // left
        }
        transform.localScale = currentScale;
    }

    void FaceAwayFromPlayer(Vector2 fleeDirection)
    {
        Vector3 currentScale = transform.localScale;
        if (fleeDirection.x > 0)
        {
            currentScale.x = Mathf.Abs(currentScale.x); // right
        }
        else
        {
            currentScale.x = -Mathf.Abs(currentScale.x); // left
        }
        transform.localScale = currentScale;
    }
////////////////////////////////////////////////////////////
/// FLEE CD
    IEnumerator FleeChanceCoolDown()
    {
        //playerANIM.SetBool("hurtAN", false);
        Debug.LogWarning("Cooldown");
        canFlee = false;
        isFleeing = false;

        sightRange = defaultSight;
        yield return new WaitForSeconds(FleeCooldown);
        canFlee = true;
        //EmergencyCDCD = false;
    }
///////////////////////////////////////////////////////////////////
////// DAMAGE COOLDOWN
    private IEnumerator InvulnerableCD()
    {
        yield return new WaitForSeconds(Whathit);
        //playerANIM.SetBool("hurtAN", false);
        vulnerable = true;
        //Debug.LogWarning("vulnerable");
    }

    private IEnumerator AnimatorHitCD()
    {
        takingDamage = false;
        yield return new WaitForSeconds(0.3f);
        //EventCallBack.HitStop.Invoke();
        enemyANIM.SetBool("hurtAN", false);
        EventCallBack.HitStop.Invoke();
        fieldOfViewAngle = defaultFieldOfViewAnge;
        sightRange = defaultSight;
        fleeRange = defaultFleeR;
    }
////////////////////////////////////////////////////////////
/// GIZMOZ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2.left * wallCheckDistance));

         //Draw a cone for the enemy's field of view
        Gizmos.color = Color.green;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle) * transform.right * sightRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle) * transform.right * sightRange;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
}