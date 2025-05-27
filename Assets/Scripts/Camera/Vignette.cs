using System.Collections;
using UnityEngine;

public class Vignette : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    
    public SceneManagerer sceneManagerer;

    private void Start()
    {
        loadingUI.SetActive(false);
    }

    private void ShowLoading()
    {
        StartCoroutine(LoadingRoutine());
    }

    private IEnumerator LoadingRoutine()
    {
        EventCallBack.OnAttack();
        sceneManagerer.LoadingScene();
        loadingUI.SetActive(true);

        // Tunggu 2 detik atau sesuaikan durasi loading
        yield return new WaitForSeconds(2f);
        sceneManagerer.LoadingScene();
        EventCallBack.EndAttack();
        loadingUI.SetActive(false);
        
        //Debug.Log("Loading selesai");
    }

    private void OnEnable()
    {
        EventCallBack.Vignette += ShowLoading;
    }

    private void OnDisable()
    {
        EventCallBack.Vignette -= ShowLoading;
    }
}
