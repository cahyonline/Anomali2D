using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    public string[] musicNames; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (string musicName in musicNames)
            {
                AudioManager.Instance.PlayMusic(musicName); 
            }
        }
    }
}