using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguesManagererAD2 : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string npcText;
        public ResponseOption[] responses;
    }

    [System.Serializable]
    public class ResponseOption
    {
        public string responseID; //////
        public string responseText;
        public int nextDialogueIndex;
        public bool isReuseable = false;
    }

        private HashSet<string> choosenResponses = new HashSet<string>(); ///////
        public TextMeshProUGUI npcDialogueText;
        public GameObject NPCPanel;
        public GameObject responsePanel;

        public Button responseButtonPrefab;
        public Transform responseContainer;
        [SerializeField] public UltimateScript ultimateScript;

        private Dialogue[] currentDialogues;
        private int currentDialogueIndex = 0;
        //public PlayerControl PlayerControllerScriptGoesHere;
        public DialogueTriggerAD2 dialogueTrigger;

    void Start()
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        
    }

    public void StartDialogue(Dialogue[] dialogues)
    {
        responsePanel.SetActive(true);
        if (dialogues == null || dialogues.Length == 0)
        {
            Debug.LogWarning("Dialogue is empty!");
            return;
        }

        currentDialogues = dialogues;
        currentDialogueIndex = 0;
        //PlayerControllerScriptGoesHere.enabled = false;
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        //if (currentDialogueIndex < 0 || currentDialogueIndex >= currentDialogues.Length)
        //{
            //EndDialogue();
            //return;
        //}

        if (currentDialogueIndex == 100)
        {
            PostponeDialogue();
            return;
            
        }

        if (currentDialogueIndex == -1)
        {
            EndDialogue();
            return;
        }

        if (currentDialogueIndex == -200)
        {
            dialogueTrigger.RestartDialogTG();
            RestartDialogAD();
            return;
        }

        if (currentDialogueIndex == -69)
        {
            ultimateScript.HasUlt();Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            dialogueTrigger.DoneDialog();
            EndDialogue();
            EventCallBack.ChangeArea(22);
            EventCallBack.ChangeAreaSpawn(48);
            EventCallBack.Vignette();
            
            return;
        }

        Dialogue currentDialogue = currentDialogues[currentDialogueIndex];
        npcDialogueText.text = currentDialogue.npcText;
        ShowResponses(currentDialogue.responses);
    }

void ShowResponses(ResponseOption[] responses)
{
    // Clear previous response buttons
    foreach (Transform child in responseContainer)
    {
        Destroy(child.gameObject);
    }

    if (responses == null || responses.Length == 0)
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        return;
    }

    List<ResponseOption> validResponses = new List<ResponseOption>();

    foreach (ResponseOption response in responses)
    {
        if (!choosenResponses.Contains(response.responseID) || response.isReuseable)
        {
            validResponses.Add(response);
        }
    }

    if (validResponses.Count == 0)
    {
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        return;
    }

    NPCPanel.SetActive(true);

    foreach (ResponseOption response in validResponses)
    {
        Button newButton = Instantiate(responseButtonPrefab, responseContainer);

        TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TextMeshProUGUI>();

        if (buttonText != null)
        {
            buttonText.text = response.responseText;
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI found in response button prefab!");
        }

        newButton.onClick.RemoveAllListeners();
        int nextDialogueIndex = response.nextDialogueIndex;
        string thisResponseID = response.responseID;

        newButton.onClick.AddListener(() =>
        {
            if (!response.isReuseable)
                choosenResponses.Add(thisResponseID);

            OnResponseSelected(nextDialogueIndex);
        });
    }
}

    void OnResponseSelected(int nextDialogue)
    {
        //if (nextDialogue < 0 || nextDialogue >= currentDialogues.Length)
        //{
            //Debug.LogWarning("Invalid dialogue index selected: " + nextDialogue);
            //EndDialogue();
            //return;
        //}
        if (nextDialogue == -1)
        {
            EndDialogue();
            Debug.LogWarning("DIALOG IS OVER");
            return;
        }

        if (nextDialogue == 100)
        {
            //choosenResponses.Clear();
            currentDialogueIndex = 0;
            Debug.LogWarning("DIALOG IS POSTPONED");
            PostponeDialogue();
        }


        Debug.Log("Player selected response leading to dialogue index: " + nextDialogue);
        currentDialogueIndex = nextDialogue;
        ShowDialogue();
    }

    void EndDialogue()
    {
        npcDialogueText.text = "";
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        Debug.Log("Dialogue OVER.");
        //PlayerControllerScriptGoesHere.enabled = true;
        dialogueTrigger.DoneDialog();

        choosenResponses.Clear(); //////////
    }
    void PostponeDialogue()
    {
        npcDialogueText.text = "";
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        Debug.Log("Dialogue POSTPONED.");
        dialogueTrigger.PostponeDialog();
        //PlayerControllerScriptGoesHere.enabled = true;
        //choosenResponses.Clear(); //////////
        //dialogueTrigger.DoneDialog();
    }

    public void RestartDialogAD()
    {
        choosenResponses.Clear(); //////////
        NPCPanel.SetActive(false);
        responsePanel.SetActive(false);
        //PlayerControllerScriptGoesHere.enabled = true;
    }

    public void RestartDIalogADData()
    {
        choosenResponses.Clear();
    }
}