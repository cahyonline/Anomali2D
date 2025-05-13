using System.Collections;
using UnityEngine;

public class Vignette : MonoBehaviour
{
    [SerializeField] private GameObject loadingUI;
    private bool isLoading = false;

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
        isLoading = true;
        loadingUI.SetActive(true);

        // Tunggu 2 detik atau sesuaikan durasi loading
        yield return new WaitForSeconds(2f);

        EventCallBack.EndAttack();
        loadingUI.SetActive(false);
        isLoading = false;
        Debug.Log("Loading selesai");
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
