using UnityEngine;

public class DialogueTriggerAD2 : MonoBehaviour
{
    public GameObject UIinteractE;
    public string finalDialog = "FUCK OFF !";
    private string defaultFinalDialog;
    public DialogueStartAD2 dialogueStartScript;
    public GameObject dialogUIparent;
    public DialogueStartAD2.Dialogue npcDialogue;
    public DialoguesManagererAD2 dialogueManager;
    private bool inRange = false;
    private bool interactingDialog;
    private bool postponeDialogCheck = false;
    static bool dialogueDone;
    public DialoguesManagererAD2.Dialogue[] dialogues;

    void Start()
    {
        UIinteractE.SetActive(false);
        interactingDialog = false;
        dialogueDone = false;
        dialogUIparent.SetActive(false);
        defaultFinalDialog = finalDialog;
    }

    void Update()
    {
        if (inRange && Input.GetKey(KeyCode.E) && !postponeDialogCheck)
        {
            dialogueStartScript.StartDialogue(npcDialogue, finalDialog);
            UIinteractE.SetActive(false);
            inRange = false;
            interactingDialog = true;
            dialogUIparent.SetActive(true);
            
            GamesState.InCutscene = true;
            EventCallBack.OnAttack();            
        }
        
        if(inRange && dialogueDone && Input.GetKey(KeyCode.E))
        {
            dialogueStartScript.StartDialogue(npcDialogue,finalDialog);
            UIinteractE.SetActive(false);
            inRange = false;
        }
        if (inRange && Input.GetKey(KeyCode.E) && postponeDialogCheck)
        {
            DialogueAD();
            UIinteractE.SetActive(false);
            inRange = false;
            interactingDialog = true;
            GamesState.InCutscene = true;
            EventCallBack.OnAttack();
            //Debug.LogWarning("PostPoned DT");
        }

        //if (interactingDialog) UIinteractE.SetActive(false);
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
            GamesState.InCutscene = false;
            EventCallBack.EndAttack();
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
        GamesState.InCutscene = true;
        EventCallBack.OnAttack();
    }

    public void DoneDialog()
    {
        dialogueDone = true;
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
    }

    public void PostponeDialog()
    {
        interactingDialog = false;
        dialogueDone = false;
        postponeDialogCheck = true;
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
    }
    public void RestartDialogTG()
    {
        interactingDialog = false;
        dialogueDone = false;
        postponeDialogCheck = false;
        dialogueStartScript.RestartStartDialog();
        GamesState.InCutscene = false;
        EventCallBack.EndAttack();
    }
}