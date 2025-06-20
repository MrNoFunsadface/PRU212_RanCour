using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform choicesContainer;
    [SerializeField] private Button choiceButtonPrefab;

    [Header("Settings")]
    [SerializeField] private float typewriterSpeed = 0.05f;

    private Story currentStory;
    private bool isDialogueActive = false;
    private bool isTyping = false;
    private List<Button> currentChoiceButtons = new List<Button>();

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(TextAsset inkJSON)
    {
        if (isDialogueActive) return;

        currentStory = new Story(inkJSON.text);
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        // Disable player movement
        if (FindObjectOfType<PlayerController>() != null)
        {
            FindObjectOfType<PlayerController>().enabled = false;
        }

        ContinueStory();
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);

        // Re-enable player movement
        if (FindObjectOfType<PlayerController>() != null)
        {
            FindObjectOfType<PlayerController>().enabled = true;
        }

        ClearChoices();
        currentStory = null;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            StartCoroutine(TypewriterEffect(nextLine));
        }
        else
        {
            if (currentStory.currentChoices.Count == 0)
            {
                EndDialogue();
            }
            else
            {
                DisplayChoices();
            }
        }
    }

    private IEnumerator TypewriterEffect(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;

        if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
    }

    private void DisplayChoices()
    {
        ClearChoices();

        foreach (Choice choice in currentStory.currentChoices)
        {
            Button choiceButton = Instantiate(choiceButtonPrefab, choicesContainer);
            choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.text;

            int choiceIndex = choice.index;
            choiceButton.onClick.AddListener(() => MakeChoice(choiceIndex));

            currentChoiceButtons.Add(choiceButton);
        }
    }

    private void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);
        ClearChoices();
        ContinueStory();
    }

    private void ClearChoices()
    {
        foreach (Button button in currentChoiceButtons)
        {
            Destroy(button.gameObject);
        }
        currentChoiceButtons.Clear();
    }

    private void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space) && !isTyping)
        {
            if (currentStory.currentChoices.Count == 0)
            {
                ContinueStory();
            }
        }
    }

    // Add to your existing DialogueManager class

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public void StartDialogueWithoutDisablingPlayer(TextAsset inkJSON)
    {
        if (isDialogueActive) return;

        currentStory = new Story(inkJSON.text);
        isDialogueActive = true;
        dialoguePanel.SetActive(true);

        // Don't disable player for cutscene dialogue
        ContinueStory();
    }
}