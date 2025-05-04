using UnityEngine;

public class PlayerSurface : MonoBehaviour
{
    public SurfaceType currentSurface = SurfaceType.Tanah;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Tanah"))
        {
            currentSurface = SurfaceType.Tanah;
            //Debug.Log("Tanah");
        }
        else if (other.collider.CompareTag("Batu"))
        {
            currentSurface = SurfaceType.Batu;
            //Debug.Log("Batu");
        }
    }
}
