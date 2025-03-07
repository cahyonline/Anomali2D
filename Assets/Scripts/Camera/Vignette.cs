using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Vignette : MonoBehaviour
{
    [SerializeField] private PostProcessVolume Menghitam;


    private IEnumerator Hitam()
    {
        while ( Menghitam.weight < 1 )
        {
            Menghitam.weight += 0.1f;
            yield return new WaitForSeconds (0.1f);
            
        }



        PlayerWake();
    }

    private IEnumerator Putih()
    {
        while (Menghitam.weight >0)
        {
            Menghitam.weight -= 0.1f;
            yield return new WaitForSeconds(0.1f);

        }

    }

    private  void PlayerFall()
    {
        StartCoroutine(Hitam());
        
    }

    private void PlayerWake()
    {
        StartCoroutine(Putih());
    }

    private void OnEnable()
    {
        EventCallBack.Vignette += PlayerFall;
        //EventCallBack.NextWorld += PlayerWake;
    }

    private void OnDisable()
    {
        EventCallBack.Vignette -= PlayerFall;
        //EventCallBack.NextWorld -= PlayerWake;
    }
}
