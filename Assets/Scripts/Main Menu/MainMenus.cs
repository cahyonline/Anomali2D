using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // biar bisa akses Button

public class MainMenus : MonoBehaviour
{
    public GameObject MenuPanel;
    public GameObject CreditPanel;
    public GameObject OptionPanel;
    public ParticleSystem particleSystem;

    [Header("UI Buttons")]
    [SerializeField] private Button LoadGameButton; // drag tombol Load Game ke sini via Inspector

    void Start()
    {
        MenuPanel.SetActive(true);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(false);
        particleSystem.Play();

        // cek apakah ada save file
        Save saver = FindFirstObjectByType<Save>();
        if (SaveUtility.HasSave())
        {
            LoadGameButton.interactable = true;
            Debug.Log("Ada save file → Load Game aktif.");
        }
        else
        {
            LoadGameButton.interactable = false;
            Debug.Log("Tidak ada save file → Load Game dimatikan.");
        }

    }

    public void StartStory()
    {
        SceneManager.LoadScene("BeforeGame");
    }

    public void LoadGame()
    {
        EventCallBack.LoadRequested = true;
        SceneManager.LoadScene("Games"); // scene gameplay
    }

    public void QuitGames()
    {
        Application.Quit();
        Debug.Log("Game telah keluar");
    }

    public void CreditButton()
    {
        MenuPanel.SetActive(false);
        CreditPanel.SetActive(true);
        OptionPanel.SetActive(false);
    }

    public void OptionButton()
    {
        MenuPanel.SetActive(false);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(true);
    }

    public void BackButton()
    {
        MenuPanel.SetActive(true);
        CreditPanel.SetActive(false);
        OptionPanel.SetActive(false);
    }
}
