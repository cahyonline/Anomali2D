using UnityEngine;
using System.Collections;
using System.Linq;

public class Ground : MonoBehaviour
{
    [SerializeField] private float Delay = 0.2f;
    public static bool isHolding = false;

    void Update()
    {
        // Cek apakah tombol "S" dan "Spasi" ditekan bersamaan
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(HoldAndDisableColliders());
            isHolding = true;
        }
    }

    private IEnumerator HoldAndDisableColliders()
    {
        //isHolding = true;

        yield return new WaitForSeconds(0f);

        Collider2D[] groundColliders = GameObject.FindGameObjectsWithTag("Tembus")
            .Select(g => g.GetComponent<Collider2D>())
            .ToArray();

        foreach (var collider in groundColliders)
        {
            if (collider != null)
            {
                collider.enabled = false;
            }
        }

        yield return new WaitForSeconds(Delay);

        foreach (var collider in groundColliders)
        {
            if (collider != null)
            {
                collider.enabled = true;
            }
        }

        isHolding = false;
    }
}