using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF";
    public DialogueStart dialogueStartManager;
    public DialogueStart.Dialogue npcDialogue;
    public DialoguesManagererAD dialogueManager;
    private bool inRange = false;
    private bool interactingDialog;
    static bool dialogueDone;
    public DialoguesManagererAD.Dialogue[] dialogues;

    void Start()
    {
        UIinteractE.SetActive(false);
        interactingDialog = false;
        dialogueDone = false;
    }

    void Update()
    {
        if(inRange && Input.GetKey(KeyCode.E))
        {
            dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            UIinteractE.SetActive(false);
            inRange = false;
            interactingDialog = true;
        }
        
        if(inRange && dialogueDone && Input.GetKey(KeyCode.E))
        {
            dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            UIinteractE.SetActive(false);
            inRange = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !interactingDialog)
        {
            //dialogueManager.StartDialogue(dialogues);
            UIinteractE.SetActive(true);
            inRange = true;
            //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            //Debug.Log("UI Show");
        }

        if (other.CompareTag("Player") && dialogueDone)
        {
            //dialogueManager.StartDialogue(dialogues);
            UIinteractE.SetActive(true);
            inRange = true;
            //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            Debug.Log("UI Show done");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
          UIinteractE.SetActive(false);  
          inRange = false;
        }
        
    }

    public void DialogueAD()
    {
        dialogueManager.StartDialogue(dialogues);
    }

    public void DoneDialog()
    {
        dialogueDone = true;
    }
}