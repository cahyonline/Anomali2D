using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ngehe : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(KOntol());
    }

    private IEnumerator KOntol()
    {
        yield return new WaitForSeconds(28f);
        SceneManager.LoadScene("Games");
    }
}
