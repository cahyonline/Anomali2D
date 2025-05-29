using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueTriggerCS3 : MonoBehaviour
{
    //public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF";
    public DialogueStartCS3 dialogueStarterCS3;
    public GameObject dialogUIparent;
    public DialogueStartCS3.Dialogue npcDialogue;

    //public DialoguesManagererNormal dialogueManager;
    //public PlayableDirector Cutscene1;
    //private bool inRange = false;
    //private bool interactingDialog;
    //static bool dialogueDone;
    //public DialoguesManagererNormal.Dialogue[] dialogues;

    void Start()
    {
        //UIinteractE.SetActive(false);
        //interactingDialog = false;
        //dialogueDone = false;
        dialogUIparent.SetActive(false);
    }

    void Update()
    {
        // if(inRange && Input.GetKey(KeyCode.E))
        // {
        //     dialogueStarterCS.StartDialogue(npcDialogue,finalDialog);
        //     UIinteractE.SetActive(false);
        //     inRange = false;
        //     interactingDialog = true;
        //     dialogUIparent.SetActive(true);
        // }
        
        // if(inRange && dialogueDone && Input.GetKey(KeyCode.E))
        // {
        //     dialogueStarterCS.StartDialogue(npcDialogue,finalDialog);
        //     UIinteractE.SetActive(false);
        //     inRange = false;
        // }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // if (other.CompareTag("Player") && !interactingDialog)
        // {
        //     //dialogueManager.StartDialogue(dialogues);
        //     //UIinteractE.SetActive(true);
        //     inRange = true;
        //     //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
        //     //Debug.Log("UI Show");
        // }

        // if (other.CompareTag("Player") && dialogueDone)
        // {
        //     //dialogueManager.StartDialogue(dialogues);
        //     //UIinteractE.SetActive(true);
        //     inRange = true;
        //     //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
        //     //Debug.Log("UI Show done");
        // }
    }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //       UIinteractE.SetActive(false);  
    //       inRange = false;
    //     }
        
    // }
    public void StartCutsceneDialog()
    {
        dialogueStarterCS3.StartDialogue(npcDialogue, finalDialog);
        //UIinteractE.SetActive(false);
        //inRange = false;
        //interactingDialog = true;
        dialogUIparent.SetActive(true);
        Debug.Log("StartCut");
    }
    public void DoneDialog()
    {
       //dialogueDone = true;
    }
}