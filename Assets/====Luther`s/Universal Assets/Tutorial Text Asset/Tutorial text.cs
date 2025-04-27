using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorialtext : MonoBehaviour
{
    // Start is called before the first frame update
    public string showText;
    public GameObject Text;
    public Animator animationFade;

    void Start()
    {
        Text.SetActive(false);
        animationFade.SetBool("showtext" , false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Text.SetActive(true);
            animationFade.SetBool("showtext" , true);

        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            animationFade.SetBool("showtext" , false);
            StartCoroutine(OffAfterOut());
        }
    }

    private IEnumerator OffAfterOut()
    {
        AnimatorStateInfo stateInfo = animationFade.GetCurrentAnimatorStateInfo(0);
        while (!stateInfo.IsName("Hide"))
        {
            yield return null;
            stateInfo = animationFade.GetCurrentAnimatorStateInfo(0);
        }
        float duration = stateInfo.length;
        yield return new WaitForSeconds(duration);
        Text.SetActive(false);
    }
}
