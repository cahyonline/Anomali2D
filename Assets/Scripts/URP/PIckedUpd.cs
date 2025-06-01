using System.Collections;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public InventoryItem itemData; 
    public int amount = 1;
    [SerializeField] private GameObject UiPickupE;
    [SerializeField] private GameObject UIPickupEnter;
    [SerializeField] private bool EInteractTerang;
    [SerializeField] private bool isInteract;
    [SerializeField] private PlayerInventory inventory;
    [SerializeField] private PlayerAnimator animHandler;
    [SerializeField] private GameObject showPaper;

    private void Start()
    {
        showPaper.SetActive(false);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && itemData != null)
        {
            EInteractTerang = true;
            UiPickupE.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EInteractTerang = false;
            UiPickupE.SetActive(false);
        }
    }

    private void Update()
    {
        if( GamesState.InCutscene ) return;
        if (EInteractTerang && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine( isTerang()); 
            EInteractTerang = false;
            UiPickupE.SetActive(false);
        }
        if (isInteract && Input.GetKeyDown(KeyCode.Return))
        {
            showPaper.SetActive(false);
            UIPickupEnter.SetActive(false);
            GamesState.InInteract = false;
            EventCallBack.EndAttack();
            StartCoroutine(destroy());

        }
    }

    private IEnumerator isTerang()
    {
        //PlayerInventory inventory = GetComponent<PlayerInventory>();

        if (inventory != null && itemData != null)
        {
            isInteract = true;
            animHandler.CollectItem = true;
            yield return new WaitForSeconds(1.1f);
            showPaper.SetActive(true);
            UIPickupEnter.SetActive(true);
            inventory.AddItem(itemData, amount);
            //Debug.Log($"Player mengambil item: {itemData.itemName} x{amount}");
            //StartCoroutine(destroy());
        }
    }
    private IEnumerator destroy()
    {
        yield return new WaitForSeconds(0.1f);
            Destroy(gameObject);
    }

}
