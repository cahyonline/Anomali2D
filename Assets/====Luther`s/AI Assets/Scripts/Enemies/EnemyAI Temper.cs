using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyAITemper : MonoBehaviour
{
////////////////////////////////////////////////////////////
/// BASIC
    public Transform player;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
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
    private float nextFireTime = 0f;
    private bool canFlee = true;
    private bool isFleeing = false;
    private bool isForceFight = false;
    private float fleeCD = 0f;
    private bool EmergencyCDCD =false;
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
////////////////////////////////////////////////////////////
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        defaultFleeR = fleeRange;
    }
////////////////////////////////////////////////////////////
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer <= fleeRange && canFlee)
        {
            StartCoroutine(Flee());
        }
        else if (distanceToPlayer <= sightRange && !isFleeing && CanSeePlayer())
        {
            FacePlayer();
            ShootProjectile();
        }
        else if (distanceToPlayer <= sightRange && CanSeePlayer() && isForceFight == true)
        {
            FacePlayer();
            ShootProjectile();
        }
        else if (distanceToPlayer <= sightRange && EmergencyCDCD)
        {
            FacePlayer();
            ShootProjectile();
        }
        else
        {
            Idle();
        }
////////////////////////////////////////////////////////////
/// VIGILANCE 
        if (fleeRange == 0 && distanceToPlayer >= sightRange)
        {
            isForceFight = false;
            fleeRange = defaultFleeR;
            EmergencyCDCD = false;
            fleeSpeed = fleeSpeed + 1f;
            defaultFleeR = defaultFleeR + 2f;
            fleeRange = fleeRange + 2f;
            FleeCooldown = FleeCooldown - 1f;
            Debug.Log("RESET FLEE CHANCE");

            if (defaultFleeR >= sightRange)
            {
                defaultFleeR = sightRange*0.5f;
                sightRange = sightRange + 2f;
                fireCD =  fireCD - 1f;

                if(fireCD <= 0 )
                {
                    fireCD = 1f;
                }
            }
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
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCD;

            GameObject newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            
            // Determine the direction based on enemy's local scale
            int direction = transform.localScale.x < 0 ? -1 : 1;
            
            // SEND DIRECTION TO PP
            newProjectile.GetComponent<ProjectileProperties>().SetDirection(direction);
        }
    }
////////////////////////////////////////////////////////////
/// FLEE DETERMINED
IEnumerator Flee()
{
    //float fleeRangeSave;
    //fleeRangeSave = fleeRange;
    fleeRange = 0f;
    canFlee = false;
    isFleeing = true;
    float defaultFleeCD = 0.25f;

    Vector2 fleeDirection = (transform.position - player.position).normalized;
    fleeDirection.y = 0; // Restrict to horizontal movement

    if (fleeDirection.magnitude < 0.1f)
    {
        fleeDirection = new Vector2(1f, 0f);
    }

    RaycastHit2D wallCheck = Physics2D.Raycast(transform.position, fleeDirection, wallCheckDistance, wallLayer);
    if (wallCheck.collider != null)
    {  
        isFleeing = false;
        canFlee = true;
        yield break; //STOP FLEE
    }

    FaceAwayFromPlayer(fleeDirection);
    Vector2 targetPosition = (Vector2)transform.position + fleeDirection * fleeSpeed * 1f; // Fleeing further
    while ((Vector2)transform.position != targetPosition)
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, fleeSpeed * Time.deltaTime);

        wallCheck = Physics2D.Raycast(transform.position, fleeDirection, wallCheckDistance, wallLayer);
        if (wallCheck.collider != null)
        {
            
            float noFlee = 0;
            fleeRange = noFlee;
            isForceFight = true;
            defaultFleeCD = fleeCD;
            fireCD =  fireCD - 1f;
            defaultFleeR = sightRange*0.5f;
            sightRange = sightRange + 2f;

            if(fireCD <= 0 )
            {
                fireCD = 1f;
            }

            break;
            
        }
        //if (wallCheck.collider != null && )
        yield return null; // Wait 4 frame
    }

    Debug.Log("Fleeing complete, returning to normal behavior.");

    //fleeRange = fleeRangeSave;
    yield return new WaitForSeconds(defaultFleeCD); // Delay before flee
    isFleeing = false;
    canFlee = true;
    fleeRange = defaultFleeR;
    StartCoroutine(FleeChanceCoolDown());
}    
////////////////////////////////////////////////////////////
    void Idle()
    {
        rb.velocity = Vector2.zero;
        //fleeRange = defaultFleeR;
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
/// GIZMOZ
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fleeRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + (Vector3)(Vector2.left * wallCheckDistance));

        // Draw a cone for the enemy's field of view
        Gizmos.color = Color.green;
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -fieldOfViewAngle) * transform.right * sightRange;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, fieldOfViewAngle) * transform.right * sightRange;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary);
    }
////////////////////////////////////////////////////////////
/// FLEE CD
    IEnumerator FleeChanceCoolDown()
    {
        Debug.LogWarning("Cooldown");
        canFlee = false;
        isFleeing = false;

        EmergencyCDCD = true;
        yield return new WaitForSeconds(FleeCooldown);
        canFlee = true;
        EmergencyCDCD = false;
    }
}