using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    public float parallaxFactor;
    private Vector3 startPos;
    private bool isPlayerInside = false; // Status pemain di dalam collider
    private float lastDelta; // Menyimpan delta terakhir

    private void Awake()
    {
        startPos = transform.localPosition;
    }

    private void OnEnable()
    {
        EventCallBack.ResetBg += ResetPosition;
    }

    private void OnDisable()
    {
        EventCallBack.ResetBg -= ResetPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInside = false;
        }
    }

    public void Move(float delta)
    {
        if (isPlayerInside) 
        {
            Vector3 newPos = transform.localPosition;
            newPos.x -= delta * parallaxFactor;
            transform.localPosition = newPos;
            lastDelta = delta;
        }
    }

    private void ResetPosition()
    {
        isPlayerInside = false;
        Vector3 pos = transform.localPosition;
        pos.x = startPos.x; 
        transform.localPosition = pos;

        lastDelta = 0; 
    }
}