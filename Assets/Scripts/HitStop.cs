using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float duration = 0.1f; // 0.1f is more appropriate for hit stop

    bool _isFrozen = false;

    public void Freeze()
    {
        if (!_isFrozen)
        {
            StartCoroutine(DoFreeze());
        }
    }

    IEnumerator DoFreeze()
    {
        _isFrozen = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
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