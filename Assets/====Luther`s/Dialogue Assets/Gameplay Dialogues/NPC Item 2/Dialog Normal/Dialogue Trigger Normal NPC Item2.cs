using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueTriggerNormalNPCItem2 : MonoBehaviour
{
    public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF";
    public string lastDialog = "Do you suck dick ?";
    public DialogueStartNormalNPCItem2 dialogueStartManager;
    public GameObject dialogUIparent;
    public DialogueStartNormalNPCItem2.Dialogue npcDialogue;
    //public DialoguesManagererNormal dialogueManager;
    //public PlayableDirector Cutscene1; 
    private bool inRange = false;
    private bool interactingDialog;
    static bool dialogueDone;
    //public DialoguesManagererNormal.Dialogue[] dialogues;

    void Start()
    {
        UIinteractE.SetActive(false);
        interactingDialog = false;
        dialogueDone = false;
        dialogUIparent.SetActive(false);
    }

    void Update()
    {
        if (inRange && Input.GetKey(KeyCode.E)) //STARTER
        {
            dialogueStartManager.StartDialogue(npcDialogue, finalDialog, lastDialog);
            UIinteractE.SetActive(false);
            inRange = false;
            interactingDialog = true;
            dialogUIparent.SetActive(true);
            GamesState.InCutscene = true;
            EventCallBack.OnAttack();
        }

        if (inRange && dialogueDone && Input.GetKey(KeyCode.E)) //REPEAT DEFAULT
        {
            dialogueStartManager.StartDialogue(npcDialogue, finalDialog, lastDialog);
            UIinteractE.SetActive(false);
            interactingDialog = true;
            inRange = false;
        }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !interactingDialog)
        {
            //dialogueManager.StartDialogue(dialogues);
            UIinteractE.SetActive(true);
            interactingDialog = true;
            inRange = true;
            //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            //Debug.Log("UI Show");
        }

        if (other.CompareTag("Player") && dialogueDone)
        {
            //dialogueManager.StartDialogue(dialogues);
            UIinteractE.SetActive(true);
            inRange = true;
            interactingDialog = true;
            GamesState.InCutscene = false;
            EventCallBack.EndAttack();
            //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            Debug.Log("UI Show done");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIinteractE.SetActive(false);
            inRange = false;
        }

    }
    public void StartCutsceneDialog()
    {
        dialogueStartManager.StartDialogue(npcDialogue, finalDialog, lastDialog);
        UIinteractE.SetActive(false);
        //inRange = false;
        interactingDialog = true;
        dialogUIparent.SetActive(true);
    }
    public void DoneDialog()
    {
        dialogueDone = true;
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
    }

    public void ReDialog()
    {
        dialogueDone = false;
        interactingDialog = false;
        
    }
}