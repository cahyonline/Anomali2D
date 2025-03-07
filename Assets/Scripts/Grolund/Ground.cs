using UnityEngine;

public class GroundController : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private bool playerHasEntered = false;
    public float Delay = 0.2f;

    void Start()
    {
        groundCollider = GetComponent<BoxCollider2D>();
        groundCollider.isTrigger = false; 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            groundCollider.enabled = false;
            Invoke("EnableCollider", 0.25f);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHasEntered = true; 
            groundCollider.isTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && playerHasEntered)
        {
            playerHasEntered = false;
            Invoke("DisableTrigger", Delay);
        }
    }
        private void DisableTrigger()
    {
        groundCollider.isTrigger = false; 
    }
}