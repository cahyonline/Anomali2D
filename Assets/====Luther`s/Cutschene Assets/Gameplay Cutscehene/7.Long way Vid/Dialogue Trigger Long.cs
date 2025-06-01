using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueTriggerLong : MonoBehaviour
{
    //public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF";
    public GameObject theTrigger;
    public DialogueStartLong dialogueStartLong;
    public GameObject dialogUIparent;
    public DialogueStartLong.Dialogue npcDialogue;
    //public DialoguesManagererNormal dialogueManager;
    public PlayableDirector CutsceneLong; 
    //private bool inRange = false;
    private bool interactingDialog;
    //static bool dialogueDone;
    //public DialoguesManagererNormal.Dialogue[] dialogues;

    void Start()
    {
        //UIinteractE.SetActive(false);
        interactingDialog = false;
        //dialogueDone = false;
        dialogUIparent.SetActive(false);
        theTrigger.SetActive(true);
    }

    void Update()
    {
        // if(inRange && Input.GetKey(KeyCode.E))
        // {
        //     dialogueStartBoss.StartDialogue(npcDialogue,finalDialog);
        //     UIinteractE.SetActive(false);
        //     inRange = false;
        //     interactingDialog = true;
        //     dialogUIparent.SetActive(true);
        // }
        
        // if(inRange && dialogueDone && Input.GetKey(KeyCode.E))
        // {
        //     dialogueStartBoss.StartDialogue(npcDialogue,finalDialog);
        //     UIinteractE.SetActive(false);
        //     inRange = false;
        // }
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !interactingDialog)
        {
            //dialogueManager.StartDialogue(dialogues);
            //UIinteractE.SetActive(true);
            //inRange = true;
            interactingDialog = true;
            CutsceneLong.Play();
            EventCallBack.OnAttack();
            GamesState.InCutscene = true;
            //StartCutsceneDialog();
            //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
            //Debug.Log("UI Show");
        }

        // if (other.CompareTag("Player") && dialogueDone)
        // {
        //     //dialogueManager.StartDialogue(dialogues);
        //     UIinteractE.SetActive(true);
        //     inRange = true;
        //     //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
        //     Debug.Log("UI Show done");
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
        dialogueStartLong.StartDialogue(npcDialogue,finalDialog);
        //UIinteractE.SetActive(false);
        //inRange = false;
        CutsceneLong.Pause();
        interactingDialog = true;
        dialogUIparent.SetActive(true);
    }
    public void DoneDialog()
    {
        //dialogueDone = true;
        CutsceneLong.Play();
        theTrigger.SetActive(false);
        //CutsceneLong.Play();
    }
}