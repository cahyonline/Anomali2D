using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoBehaviour
{
    public Vector2 pos;
    public float SavedHealth;
    public int SavedArea;

    public void LoaderSave(SaveData data)
    {
        EventCallBack.Load(data);
        pos = data.checkpointPosition;
        SavedArea = data.Area;
        SavedHealth = data.HealthInfor;
    }


}
