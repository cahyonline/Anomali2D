using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class DialogueStartCS : MonoBehaviour
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
    public DialogueTriggerCS dialogueTriggerCS; // Reference to DialogueTrigger script
    //public PlayerControl PlayerControllerScriptGoesHere;
    public CutscenePlayer cutscenePlayer;
    //public ComboCharacter comboCharacter;

    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    private bool hasTalkedBefore = false; //////////////////////////////////////////////////
    private string repeatLine = ""; // Will be set from DialogueTrigger

    void Start()
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue newDialogue, string repeatText)
    {
        if (hasTalkedBefore) 
        {
            //EventCallBack.OnAttack();
            repeatLine = repeatText;
            ShowRepeatLine();
            return;
        }

        NPCPanel.SetActive(true);

        if (newDialogue == null || newDialogue.npcLines.Length == 0)
        {
            Debug.LogWarning("No dialogue lines available!");
            return;
        }

        currentDialogue = newDialogue;
        currentLineIndex = 0;
        isDialogueActive = true;
        //PlayerControllerScriptGoesHere.enabled = false;
        //comboCharacter.enabled = false;

        ShowNextLine();
    }

    void Update() //////////////////////////////////////////////////////////////////////////////////////
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return)) 
        {
            ShowNextLine();
            //cutscene1Player.ResumeCutscene();
        }
    }

    public void ShowNextLine() //////////////////////////////////////////////////////////////////////
    {
        if (currentLineIndex < currentDialogue.npcLines.Length)
        {
            NPCPanel.SetActive(true);
            dialogueText.text = currentDialogue.npcLines[currentLineIndex];
            currentLineIndex++;
            cutscenePlayer.PauseCutscene();
        }
        else
        {
            hasTalkedBefore = true;
            EndDialogue();
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
        cutscenePlayer.ResumeCutscene();
        isDialogueActive = false;
        //dialogueTriggerNormal.DialogueAD(); ////////////////////// EXTEND DIALOGUE TO OPTIONS DIALOG AD
        NPCPanel.SetActive(false);  ////////////////////// DIsable this if extended DIALOG AD
        responsePanel.SetActive(false); ////////////////// DIsable this if extended DIALOG AD
        //Debug.Log("Dialogue ended.");
        //PlayerControllerScriptGoesHere.enabled = true; /// DIsable this if extended DIALOG AD
        //comboCharacter.enabled = true;
        dialogueTriggerCS.DoneDialog();
    }
    public void HideTextBox()
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
    }
    

}