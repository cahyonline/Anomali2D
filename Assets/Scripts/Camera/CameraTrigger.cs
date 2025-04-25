using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private int areaIndex;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            EventCallBack.ChangeArea(areaIndex);
            EventCallBack.Vignette();
            EventCallBack.ResetBg();
        }
    }
}