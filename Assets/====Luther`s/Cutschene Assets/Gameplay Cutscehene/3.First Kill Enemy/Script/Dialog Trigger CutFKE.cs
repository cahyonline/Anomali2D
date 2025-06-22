using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueTriggerCSFKE : MonoBehaviour
{
    //public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF";
        public DialogueStartCSFKE dialogueStarterCSFKE;
    public GameObject dialogUIparent;
    public EnemyAIRanged enemyAIRanged;
    public GameObject porto3;
    public DialogueStartCSFKE.Dialogue npcDialogue;
    
    //public DialoguesManagererNormal dialogueManager;
    //public PlayableDirector CutscenePlayer2; 
    //private bool inRange = false;
    //private bool interactingDialog;
    private bool startBabel = false;
    //static bool dialogueDone;
    ////public DialoguesManagererNormal.Dialogue[] dialogues;

    void Start()
    {
        //UIinteractE.SetActive(false);
        //interactingDialog = false;
        //dialogueDone = false;
        dialogUIparent.SetActive(false);
        startBabel = false;
        porto3.SetActive(true);
    }

    void Update()
    {
        //     if(inRange && Input.GetKey(KeyCode.E))
        //     {
        //         //dialogueStarterCS1.StartDialogue(npcDialogue,finalDialog);
        //         //UIinteractE.SetActive(false);
        //         inRange = false;
        //         interactingDialog = true;
        //         dialogUIparent.SetActive(true);
        //     }

        //     if(inRange && dialogueDone && Input.GetKey(KeyCode.E))
        //     {
        //         //dialogueStarterCS1.StartDialogue(npcDialogue,finalDialog);
        //         //UIinteractE.SetActive(false);
        //         inRange = false;
        //     }

        if (enemyAIRanged.healthAmount <= 0f && startBabel == false)
        {
            StartCutsceneDialog();
            GamesState.InCutscene = true;
            EventCallBack.OnAttack();
        }

     }
    // void OnTriggerStay2D(Collider2D other)
    // {
    //     if (other.CompareTag("Player") && !interactingDialog)
    //     {
    //         //dialogueManager.StartDialogue(dialogues);
    //         //UIinteractE.SetActive(true);
    //         inRange = true;
    //         //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
    //         //Debug.Log("UI Show");
    //     }

    //     if (other.CompareTag("Player") && dialogueDone)
    //     {
    //         //dialogueManager.StartDialogue(dialogues);
    //         //UIinteractE.SetActive(true);
    //         inRange = true;
    //         //dialogueStartManager.StartDialogue(npcDialogue,finalDialog);
    //         //Debug.Log("UI Show done");
    //     }
    // }
    // void OnTriggerExit2D(Collider2D other)
    // {
    //     if(other.CompareTag("Player"))
    //     {
    //       //UIinteractE.SetActive(false);  
    //       inRange = false;
    //     }

    // }
    public void StartCutsceneDialog()
    {
        startBabel = true;
        dialogueStarterCSFKE.StartDialogue(npcDialogue, finalDialog);
        //UIinteractE.SetActive(false);
        //inRange = false;
        //interactingDialog = true;
        dialogUIparent.SetActive(true);
        porto3.SetActive(true);
        //Debug.Log("called");
    }
    public void DoneDialog()
    {
        //dialogueDone = true;
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
        porto3.SetActive(false);

    }
}