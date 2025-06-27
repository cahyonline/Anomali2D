using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Endingsss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Endingssss());
    }
    private void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
    private IEnumerator Endingssss()
    {
        yield return new WaitForSeconds(30f);
        SceneManager.LoadScene("MainMenu");
        //
    }
}
