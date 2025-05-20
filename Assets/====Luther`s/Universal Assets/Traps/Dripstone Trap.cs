using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DripstoneTrap : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D trapRB;
    public Animator animator;
    public float SpawnCooldown;
    public GameObject dripLocation;
    public GameObject dripObject;

    void Start()
    {
        trapRB.simulated = false;
        dripObject.transform.position = dripLocation.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            trapRB.simulated = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("RockATKHB"))
        {
            animator.SetBool("isFade", true);
            StartCoroutine(ResetAgain());
        }
    }

    IEnumerator ResetAgain()
    {
        yield return new WaitForSeconds(2);
        animator.SetBool("isFade", false);
        dripObject.SetActive(false);
        Start();
        yield return new WaitForSeconds(SpawnCooldown);
        animator.SetBool("isFade", false);
        dripObject.SetActive(true);
    }
}
