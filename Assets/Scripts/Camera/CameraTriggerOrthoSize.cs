using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTriggerOrthoSize : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCam;
    //[SerializeField] private float targetOrthoSize = 5.6f;
    //[SerializeField] private float transitionDuaration = 2f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            EventCallBack.RubahOrtho(5.6f, 2f);

            //Debug.Log("sdghj");
            //StartCoroutine(ChangeOrthoSize(targetOrthoSize, transitionDuaration));
        }
    }

    private IEnumerator ChangeOrthoSize(float newSize, float duration)
    {
        float startSize = virtualCam.m_Lens.OrthographicSize;
        float time = 0f;

        while (time < duration)
        {
            virtualCam.m_Lens.OrthographicSize = Mathf.Lerp(startSize, newSize, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        virtualCam.m_Lens.OrthographicSize = newSize;
    }
    private void HandleRubahOrtho(float size, float duration)
    {
        StartCoroutine(ChangeOrthoSize(size, duration));
    }

    private void OnEnable()
    {
        EventCallBack.RubahOrtho += HandleRubahOrtho;
    }

    private void OnDisable()
    {
        EventCallBack.RubahOrtho -= HandleRubahOrtho;
    }

}
