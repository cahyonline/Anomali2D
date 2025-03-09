using UnityEngine;

public class ProjectileProperties : MonoBehaviour
{
    public float speed = 5f;
    private int direction = 1; // Default right
    public bool destroyImpactP = true;

    // Set the direction based on the enemy's facing direction
    public void SetDirection(int dir)
    {
        direction = dir;
        Vector3 scaler = transform.localScale;
        scaler.x = direction;
        transform.localScale = scaler; // Flip if necessary
    }

    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(speed * direction, 0);
        Destroy(gameObject,10f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && destroyImpactP)
        {
            Destroy(gameObject,0.1f);
        }
        if(other.CompareTag("WallsGrounds"))
        {
            Destroy(gameObject,0.1f);
        }
    }
}