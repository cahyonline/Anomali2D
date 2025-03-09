using UnityEngine;
using System.Collections;
using System.Linq;

public class Ground : MonoBehaviour
{
    public float Delay = 0.5f;
    private bool isHolding = false;

    void Update()
    {
        // Cek apakah tombol "S" dan "Spasi" ditekan bersamaan
        if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.Space) && !isHolding)
        {
            StartCoroutine(HoldAndDisableColliders());
        }
    }

    private IEnumerator HoldAndDisableColliders()
    {
        isHolding = true;

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