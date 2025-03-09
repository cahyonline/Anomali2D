using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    public Transform player; // Assign the player's Transform in the Inspector
    public LayerMask playerLayer; // Assign only the "Player" layer in the Inspector
    public float maxSightRange = 10f; // Maximum detection range

    void Update()
    {
        ShootRay();
    }

    void ShootRay()
    {
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Limit sight range to maxSightRange (10f) if player is too far
        float sightRange = Mathf.Min(distanceToPlayer, maxSightRange);

        // Cast ray that only detects the "Player" layer
        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToPlayer, sightRange, playerLayer);

        // Draw the ray for debugging
        Debug.DrawRay(transform.position, directionToPlayer * sightRange, Color.red);

        if (hit.collider != null)
        {
            Debug.Log("Ray hit: " + hit.collider.name);

            if (hit.collider.CompareTag("Player"))
            {
                Debug.Log("Ray stopped at the Player!");
            }
        }
    }
}