using UnityEngine;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private GameObject Cutscene1;
    void Start()
    {
        // kalau ada request load dari MainMenu
        if (EventCallBack.LoadRequested)
        {
            Cutscene1.SetActive(false);
            // panggil sistem load (Save.cs akan ikut jalan)
            EventCallBack.LoadGame?.Invoke();
            EventCallBack.LoadRequested = false;
            Debug.Log("GameInitializer: LoadGame dipanggil.");
        }

        // disable semua anak (cutscene dsb.)
        //foreach (Transform child in transform)
        //{
        //   Cutscene1.SetActive(false);
        //}
        //Debug.Log("GameInitializer: semua child cutscene dimatikan.");
    }
}
