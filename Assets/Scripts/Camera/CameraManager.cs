using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Collider2D Area2;
    [SerializeField] private CinemachineConfiner2D cameras;


    private void OnTriggerExit2D(Collider2D collision)
    {
        EventCallBack.PlayerFalling();
    }
    private void Hajiemas()
    {
        cameras.m_BoundingShape2D = Area2;

    }

    private void OnEnable()
    {
        EventCallBack.NextWorld += Hajiemas;
    }

    private void OnDisable()
    {
        EventCallBack.NextWorld -= Hajiemas;
    }
}
