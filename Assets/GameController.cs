using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    Vector2 checkpointPos;
    public Rigidbody2D playerRb;

    
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {
        checkpointPos = transform.position;
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Kontol"))
        {
            //Debug.LogWarning("asdgh");
            Die();
        }
    }
    public void UpdateCheckpoint(Vector2 pos)
    {
        checkpointPos = pos;
    }
    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }
    IEnumerator Respawn(float duration)
    {
        playerRb.velocity = new Vector2(0, 0);
        playerRb.simulated = false;
        transform.localScale = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(duration);
        transform.position = checkpointPos;
        transform.localScale = new Vector3(1, 1, 1);
        playerRb.simulated = true;
    }
}
