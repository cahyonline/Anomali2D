using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    private static bool hasPlayed = false;

    public AudioSource audioSource;

    private void Start()
    {
        // Pastikan ini hanya dipanggil di MainMenu
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            if (!hasPlayed)
            {
                hasPlayed = true;
                audioSource.Play();
            }
            else
            {
                // Stop and replay if already played before (prevent overlapping)
                audioSource.Stop();
                audioSource.Play();
            }
        }
    }
}
