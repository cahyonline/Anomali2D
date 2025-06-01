using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class DialogueTriggerEnding1 : MonoBehaviour
{
    //public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF";
    public DialogueStartEnding1 dialogueStarterFalling;
    public GameObject RootCutscene;
    public PlayableDirector CutscenePlayerEnd;
    public GameObject dialogUIparent;
    //public EnemyAIRanged enemyAIRanged;
    public DialogueStartEnding1.Dialogue npcDialogue;


    //public DialoguesManagererNormal dialogueManager;

    //private bool inRange = false;
    //private bool interactingDialog;
    //private bool startBabel = false;
    //static bool dialogueDone;
    ////public DialoguesManagererNormal.Dialogue[] dialogues;

    void Start()
    {
        //UIinteractE.SetActive(false);
        //interactingDialog = false;
        //dialogueDone = false;
        dialogUIparent.SetActive(false);
        //startBabel = false;
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

        // if (enemyAIRanged.healthAmount <= 0f && startBabel == false)
        // {
        //     StartCutsceneDialog();
        //     GamesState.InCutscene = true;
        //     EventCallBack.OnAttack();
        // }

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


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CutscenePlayerEnd.Play();
            GamesState.InCutscene = true;
            EventCallBack.OnAttack();
            //StartCutsceneDialog();
        }
    }
    // }
    public void StartCutsceneDialog()
    {
        //startBabel = true;
        dialogueStarterFalling.StartDialogue(npcDialogue, finalDialog);
        //UIinteractE.SetActive(false);
        //inRange = false;
        //interactingDialog = true;
        dialogUIparent.SetActive(true);
        CutscenePlayerEnd.Pause();
        Debug.Log("called");
    }
    public void DoneDialog()
    {
        //dialogueDone = true;
        GamesState.InCutscene = false;
        //CutscenePlayerEnd.Stop();
        EventCallBack.EndAttack();
        //
    }

    public void DestroyThisObject()
    {
        RootCutscene.SetActive(false);
    }
}