using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueStartNormalNPCItem : MonoBehaviour
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
    [SerializeField] private PlayerInventory playerInventory; public InventoryItem inventoryItem; public int amount;
    
    public DialogueTriggerNormalNPCItem dialogueTriggerNormal; // Reference to DialogueTrigger script
    //public PlayerControl PlayerControllerScriptGoesHere;
    //public ComboCharacter comboCharacter;

    private Dialogue currentDialogue;
    private int currentLineIndex = 0;
    private bool isDialogueActive = false;
    //private bool hasTalkedBefore = false; //////////////////////////////////////////////////
    public bool itemCheck1;
    private bool checker1;
    private int phaseTalk = 0;
    private string repeatLine = ""; // Will be set from DialogueTrigger

    void Start()
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        checker1 = false;
        phaseTalk = 0;
        currentLineIndex = 0;
    }

    public void StartDialogue(Dialogue newDialogue, string repeatText, string repeatText2)
    {
        //////////////////////////////////////////////////////////////////////
        if (phaseTalk == 1 && !itemCheck1)
        {
            //EventCallBack.OnAttack();
            repeatLine = repeatText;
            ShowRepeatLine();
            return;
        }

        if (phaseTalk == 1 && itemCheck1)
        {
            //currentLineIndex++;
            NPCPanel.SetActive(true);
            currentLineIndex = 5;
            isDialogueActive = true;
            GamesState.InInteract = true;
            ShowNextLine();
            return;
        }

        //////////////////////////////////////////////////////////////////////
            if (phaseTalk == 2)
            {
                //EventCallBack.OnAttack();
                repeatLine = repeatText2;
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
        //currentLineIndex = 0;
        isDialogueActive = true;
        //PlayerControllerScriptGoesHere.enabled = false;
        GamesState.InInteract = true;
        //comboCharacter.enabled = false;

        ShowNextLine();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Return))
        {
            ShowNextLine();
        }

        if (itemCheck1 && !checker1)
        {
            checker1 = true;
            dialogueTriggerNormal.ReDialog();
            //Debug.Log("REupdate = " + currentLineIndex);
        }

        //Debug.Log(currentLineIndex);
    }

    void ShowNextLine()
    {
        if (currentLineIndex < currentDialogue.npcLines.Length)
        {
            dialogueText.text = currentDialogue.npcLines[currentLineIndex];
            currentLineIndex++;
        }

        if (currentLineIndex == 4)
        {
            //hasTalkedBefore = true;
            //currentLineIndex++;
            phaseTalk = 1;
            //currentLineIndex = +1;
            EndDialogue();
        }

        if (currentLineIndex == 9) ////
        {
            phaseTalk = 2;
            //lastTalk = true;
            playerInventory.AddItem(inventoryItem, amount);
            Debug.LogWarning(inventoryItem.itemName);
            EndDialogue();
        }
        else if (currentLineIndex > currentDialogue.npcLines.Length)
        {
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
        //dialogueTriggerNormal.DialogueAD(); ////////////////////// EXTEND DIALOGUE TO OPTIONS DIALOG AD
        NPCPanel.SetActive(false);  ////////////////////// DIsable this if extended DIALOG AD
        responsePanel.SetActive(false); ////////////////// DIsable this if extended DIALOG AD
        //Debug.Log("Dialogue ended.");
        //PlayerControllerScriptGoesHere.enabled = true; /// DIsable this if extended DIALOG AD
        //comboCharacter.enabled = true;
        GamesState.InInteract = false;
        dialogueTriggerNormal.DoneDialog();
    }
}