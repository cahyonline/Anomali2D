using System.Collections;
using UnityEngine;

public class HitStop : MonoBehaviour
{
    public float duration = 0.1f;
    public float cooldown = 1f;         

    private bool _isFrozen = false;
    private bool _isOnCooldown = false;

    public void Freeze()
    {
        // if (!_isFrozen && !_isOnCooldown)
        // {
        //     StartCoroutine(DoFreeze());
        // }
    }

    IEnumerator DoFreeze()
    {
        yield return new WaitForSeconds(0.3f);
        _isFrozen = true;
        float originalTimeScale = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = originalTimeScale;
        _isFrozen = false;

        // Mulai cooldown setelah freeze
        StartCoroutine(CooldownRoutine());
    }

    IEnumerator CooldownRoutine()
    {
        _isOnCooldown = true;
        yield return new WaitForSecondsRealtime(cooldown);
        _isOnCooldown = false;
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
