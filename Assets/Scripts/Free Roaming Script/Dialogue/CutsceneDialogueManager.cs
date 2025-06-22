using UnityEngine;
using TMPro;
using Ink.Runtime;
using System.Collections;

public class CutsceneDialogueManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;

    [Header("Dialogue Settings")]
    public TextAsset inkJSONAsset;
    public float autoAdvanceDelay = 2f;

    private Story story;

    public void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        story = new Story(inkJSONAsset.text);
        StartCoroutine(AutoPlayDialogue());
    }

    IEnumerator AutoPlayDialogue()
    {
        while (story.canContinue)
        {
            string text = story.Continue().Trim();
            dialogueText.text = text;
            yield return new WaitForSeconds(autoAdvanceDelay);
        }

        EndDialogue();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }
}
