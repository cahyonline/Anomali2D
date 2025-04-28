using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueStart : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        [TextArea(2, 5)] 
        public string[] npcLines;
    }

    public TextMeshProUGUI dialogueText;
    public GameObject NPCPanel;
    public GameObject responsePanel;
    public DialogueTrigger dialogueTrigger; // Reference to DialogueTrigger script
    public PlayerControl PlayerControllerScriptGoesHere;
    //public ComboCharacter comboCharacter;

    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool showStarterLine = true;
    
    private bool showDefaultLine = false; //////////////////////////////////////////////////
    private string repeatLine = ""; // Will be set from DialogueTrigger

    void Start()
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue newDialogue, string repeatText)
    {
        if (showDefaultLine) 
        {
            //EventCallBack.OnAttack();
            repeatLine = repeatText;
            ShowRepeatLine();
            //EndDialogue();
            return;
        }

        if (showStarterLine)
        {
            NPCPanel.SetActive(true);

            if (newDialogue == null || newDialogue.npcLines.Length == 0)
            {
                Debug.LogWarning("No dialogue lines available!");
                return;
            }

            currentDialogue = newDialogue;
            currentLineIndex = 0;
            isDialogueActive = true;
            PlayerControllerScriptGoesHere.enabled = false;
            //comboCharacter.enabled = false;

            ShowNextLine();            
        }

    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return)) 
        {
            ShowNextLine();
        }
    }

    void ShowNextLine()
    {
        if (currentLineIndex < currentDialogue.npcLines.Length) // SHow dialog if still available
        {
            dialogueText.text = currentDialogue.npcLines[currentLineIndex];
            currentLineIndex++;
            showStarterLine = false;
        }

        else
        {
            showDefaultLine = true;
            dialogueTrigger.DialogueAD();
            isDialogueActive = false;
        }
    }

    void ShowRepeatLine()
    {
        NPCPanel.SetActive(true);
        dialogueText.text = repeatLine;
        StartCoroutine(ClosePanelAfterDelay());
    }

    IEnumerator ClosePanelAfterDelay()
    {
        yield return new WaitForSeconds(1.5f);
        NPCPanel.SetActive(false);
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        //dialogueTrigger.DialogueAD(); ////////////////////// EXTEND DIALOGUE TO OPTIONS DIALOG AD
        //NPCPanel.SetActive(false);  ////////////////////// DIsable this if extended DIALOG AD
        //responsePanel.SetActive(false); ////////////////// DIsable this if extended DIALOG AD
        //Debug.Log("Dialogue ended.");
        //PlayerControllerScriptGoesHere.enabled = true; /// DIsable this if extended DIALOG AD
        //comboCharacter.enabled = true;
        dialogueTrigger.DoneDialog();

    }
    public void RestartStartDialog()
    {
        isDialogueActive = false;
        showStarterLine = true;
        showDefaultLine = false;
    }
}