using System.Collections;
using UnityEngine;

public class TilesBreak : MonoBehaviour
{
    private bool isBreaking = false;
    private Animator tilesAnim;
    private Collider2D tileCollider;
    [SerializeField] private GameObject Asaps;

    private void Start()
    {
        tilesAnim = GetComponent<Animator>();
        tileCollider = GetComponent<Collider2D>();
        tileCollider.enabled = true; 
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (isBreaking) return;
            tilesAnim.SetTrigger("Break");

            StartCoroutine(Breaking());
        }
    }

    private IEnumerator Breaking()
    {
        isBreaking = true;
        //tilesAnim.SetTrigger("Break");

        yield return new WaitForSeconds(1.2f);
        //tileCollider.enabled = false; 

        yield return new WaitForSeconds(1f);
        tilesAnim.SetTrigger("TilesRespawn");

        yield return new WaitForSeconds(1f);
        //tileCollider.enabled = true; 

        isBreaking = false;
    }

    private void Asap()
    {
        Instantiate(Asaps,transform.position, Quaternion.identity);
        if (Asaps != null)
        {
            GameObject effect = Instantiate(Asaps, transform.position, Quaternion.identity);
            Destroy(effect, 1f);
        }

    }

    private void WOi()
    {
        isBreaking = false;
    }
}
