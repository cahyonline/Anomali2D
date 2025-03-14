using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialoguesManagererAD : MonoBehaviour
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
        public string responseText;
        public int nextDialogueIndex;
    }

        public TextMeshProUGUI npcDialogueText;
        public GameObject NPCPanel;
        public GameObject responsePanel;

        public Button responseButtonPrefab;
        public Transform responseContainer;

        private Dialogue[] currentDialogues;
        private int currentDialogueIndex = 0;
        public PlayerControl PlayerControllerScriptGoesHere;
        public DialogueTrigger dialogueTrigger;

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
        ShowDialogue();
    }

    public void ShowDialogue()
    {
        if (currentDialogueIndex < 0 || currentDialogueIndex >= currentDialogues.Length)
        {
            EndDialogue();
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

        NPCPanel.SetActive(true);

        foreach (ResponseOption response in responses)
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
            newButton.onClick.AddListener(() => OnResponseSelected(nextDialogueIndex));
        }
    }

    void OnResponseSelected(int nextDialogue)
    {
        if (nextDialogue < 0 || nextDialogue >= currentDialogues.Length)
        {
            Debug.LogWarning("Invalid dialogue index selected: " + nextDialogue);
            EndDialogue();
            return;
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
        Debug.Log("Dialogue ended.");
        PlayerControllerScriptGoesHere.enabled = true;
        dialogueTrigger.DoneDialog();
    }
}