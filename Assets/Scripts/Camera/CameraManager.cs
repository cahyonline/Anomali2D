using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private List<Collider2D> cameraAreas; 
    [SerializeField] private CinemachineConfiner2D cameras;

    private IEnumerator WaitForHitam(int areaIndex)
    {
        yield return new WaitForSeconds(1f); 
        ChangeCameraArea(areaIndex); 
    }

    private void ChangeCameraArea(int areaIndex)
    {
        if (areaIndex >= 0 && areaIndex < cameraAreas.Count)
        {
            cameras.m_BoundingShape2D = cameraAreas[areaIndex];
        }
        else
        {
            Debug.LogWarning("Area index out of range!");
        }
    }

    private void OnEnable()
    {
        EventCallBack.ChangeArea += OnChangeArea;
    }

    private void OnDisable()
    {
        EventCallBack.ChangeArea -= OnChangeArea;
    }

    private void OnChangeArea(int areaIndex)
    {
        StartCoroutine(WaitForHitam(areaIndex)); 
    }
}