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
    public PlayerMovement PlayerControllerScriptGoesHere;
    public ComboCharacter comboCharacter;

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
        PlayerControllerScriptGoesHere.enabled = false;
        comboCharacter.enabled = false;

        ShowNextLine();
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
        if (currentLineIndex < currentDialogue.npcLines.Length)
        {
            dialogueText.text = currentDialogue.npcLines[currentLineIndex];
            currentLineIndex++;
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
        isDialogueActive = false;
        //dialogueTrigger.DialogueAD();
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        //Debug.Log("Dialogue ended.");
        PlayerControllerScriptGoesHere.enabled = true;
        comboCharacter.enabled = true;
        dialogueTrigger.DoneDialog();

    }
}