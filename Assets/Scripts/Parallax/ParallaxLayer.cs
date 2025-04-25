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
        if (other.CompareTag("Player")) // Pastikan objek pemain memiliki tag "Player"
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
        if (isPlayerInside) // Hanya bergerak jika pemain di dalam collider
        {
            Vector3 newPos = transform.localPosition;
            newPos.x -= delta * parallaxFactor;
            transform.localPosition = newPos;
            lastDelta = delta; // Simpan delta terakhir
        }
    }

    private void ResetPosition()
    {
        Vector3 pos = transform.localPosition;
        pos.x = startPos.x; // cuma reset X
        transform.localPosition = pos;

        // Reset delta
        lastDelta = 0; // Atau logika lain sesuai kebutuhan
    }
}