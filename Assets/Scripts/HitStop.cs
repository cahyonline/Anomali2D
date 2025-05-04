using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float duration = 1f;

    bool _isFrozen = false;
    float _pendingFreezeDuration = 0f;

    private void Update()
    {
        if (_pendingFreezeDuration > 0 && !_isFrozen)
        {
            StartCoroutine(DoFreezew());
        }
    }

    public void Freeze()
    {
        _pendingFreezeDuration = duration;
        //Debug.LogWarning("fcku");
    }

    IEnumerator DoFreezew()
    {
        //Debug.LogWarning("fcku");

        _isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        _pendingFreezeDuration = 0;
        _isFrozen = false;
    }

    private void OnEnable()
    {
        EventCallBack.HitStop += Freeze;
    }

    private void OnDisable()
    {
        EventCallBack.HitStop -= Freeze;
    }
}