using UnityEngine;

public class AreaTrigger : MonoBehaviour
{
    [SerializeField] private int areaIndex; // Indeks area yang dipicu

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventCallBack.ChangeAreaSpawn(areaIndex); // Panggil event ChangeAreaSpawn dengan indeks area
        }
    }
}