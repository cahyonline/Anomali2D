using UnityEngine;

public class CameraPrevTrigger : MonoBehaviour
{
    [SerializeField] private int prevAreaIndex; 

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventCallBack.ChangeArea(prevAreaIndex);
            EventCallBack.Vignette();

        }
    }
}