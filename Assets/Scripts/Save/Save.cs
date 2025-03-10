using System.IO;
using UnityEngine;

public class Save : MonoBehaviour
{
    [SerializeField] private GameObject Paused;
    private string savePath;
    public GameController Loaderghjv;
    public HealthPlayer Loadersjcghxfvsdhjg;

    
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogFormat("Saved");
        SaveGame(collision.transform.position, Loaderghjv.currentArea, Loadersjcghxfvsdhjg.healthAmount);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Load();
        }
    }
    public void Load()
    {
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
}

[System.Serializable]
public class SaveData
{
    public Vector3 checkpointPosition;
    public int Area;
    public float HealthInfor;
}