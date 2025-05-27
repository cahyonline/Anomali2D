using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Save : MonoBehaviour
{
    [SerializeField] private GameObject Paused;
    private string savePath;
    public GameController _saveLastPoss;
    public HealthPlayer _saveHealthPlayered;
    [SerializeField] private GameObject UIpickupE;
    public bool EInteract;
    private Transform playerTransform;
    public PlayerAnimator AnimHandler;


    private void Start()
    {
        //AnimHandler = GetComponent<PlayerAnimator>();
        UIpickupE.SetActive(false);
    }

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
    }

    public void SaveGame(Vector3 checkpointPosition,int area,float HealtInfor)
    {
        SaveData data = new SaveData
        {
            checkpointPosition = checkpointPosition,
            Area = area,
            HealthInfor = HealtInfor
        };

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return null;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EInteract = true;
            UIpickupE.SetActive(true);
            playerTransform = collision.transform;
        }
        
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EInteract = false;
            UIpickupE.SetActive(false);
            //Debug.Log("OUSIDE");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            
            EventCallBack.LoadGame();
        }
        if (EInteract && Input.GetKey(KeyCode.E))
        {
            EventCallBack.OnAttack();
            AnimHandler.InteractE = true;
            EInteract = false;
            UIpickupE.SetActive(false);
            SaveGame(playerTransform.transform.position, _saveLastPoss.currentArea, _saveHealthPlayered.healthAmount);
            Debug.LogFormat("Saved");
           
        }
    }
    public void Load()
    {
        EventCallBack.Vignette();
        SaveData data = LoadGame();
        if (data != null)
        {
            Load LoadSave = FindFirstObjectByType<Load>();
            if (LoadSave != null)
            {
                LoadSave.LoaderSave(data);
            }
            else
            {
                Debug.LogError("Gapunya save!");
            }
        }
    }
    private void OnEnable()
    {
        EventCallBack.LoadGame += Load;
    }
    private void OnDisable()
    {
        EventCallBack.LoadGame -= Load;
        
    }


}

[System.Serializable]
public class SaveData
{
    public Vector3 checkpointPosition;
    public int Area;
    public float HealthInfor;
    //
}