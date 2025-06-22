using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Ink.Runtime;
using TMPro;
using System;

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
    private Coroutine typewriterCoroutine;
    private string currentText = "";

    // Callback for when dialogue ends
    private Action onDialogueEnd;
    private bool wasFirstTimeDialogue = false;

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
        StartDialogueWithState(inkJSON, false);
    }

    public void StartDialogueWithState(TextAsset inkJSON, bool hasTalkedBefore, Action onDialogueEndCallback = null)
    {
        if (isDialogueActive) return;

        currentStory = new Story(inkJSON.text);

        // Set the has_talked_before variable in the Ink story
        currentStory.variablesState["has_talked_before"] = hasTalkedBefore;

        // Store callback and track if this is a first-time dialogue
        onDialogueEnd = onDialogueEndCallback;
        wasFirstTimeDialogue = !hasTalkedBefore;

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

        // Stop any running typewriter effect
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }
        isTyping = false;

        // Call the callback if this was a first-time dialogue
        if (wasFirstTimeDialogue && onDialogueEnd != null)
        {
            onDialogueEnd.Invoke();
        }

        // Reset callback
        onDialogueEnd = null;
        wasFirstTimeDialogue = false;
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            string nextLine = currentStory.Continue();
            typewriterCoroutine = StartCoroutine(TypewriterEffect(nextLine));
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
        currentText = text;
        dialogueText.text = "";

        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
        typewriterCoroutine = null;

        if (currentStory.currentChoices.Count > 0)
        {
            DisplayChoices();
        }
    }

    private void SkipTypewriter()
    {
        if (isTyping && typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
            dialogueText.text = currentText;
            isTyping = false;

            if (currentStory.currentChoices.Count > 0)
            {
                DisplayChoices();
            }
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
        if (isDialogueActive && Input.GetKeyDown(KeyCode.Space))
        {
            if (isTyping)
            {
                // Skip the typewriter effect
                SkipTypewriter();
            }
            else if (currentStory.currentChoices.Count == 0)
            {
                // Continue to next dialogue line
                ContinueStory();
            }
        }
    }

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