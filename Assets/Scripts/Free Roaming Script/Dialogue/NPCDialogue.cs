using System.Collections;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private TextAsset inkDialogueFile;                 // Default dialogue
    [SerializeField] private TextAsset inkDialogueFileAfterConditionMet; // Dialogue after condition is met
    [SerializeField] private string npcName = "Villager";
    [SerializeField] private string npcID;

    [Header("State Condition")]
    [Tooltip("Key used to check if the condition for changing state is met. E.g. 'DoorOpened', 'QuestXCompleted'")]
    [SerializeField] private string stateConditionKey;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private int interactionPriority = 1;

    [Header("State Change Settings")]
    [Tooltip("Disable the NPC GameObject after state-changing dialogue completes.")]
    [SerializeField] private bool disableAfterStateChange = true;

    [Tooltip("Fade screen to black when NPC disappears.")]
    [SerializeField] private bool hideWithFade = true;

    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Assign black panel with CanvasGroup for fading

    private bool playerInRange = false;
    private Transform player;
    private bool hasBeenTalkedTo = false;
    private bool hasChangedState = false;
    private bool isRegistered = false;

    private IConditionChecker conditionChecker;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;
        conditionChecker = new PlayerPrefsConditionChecker();

        if (string.IsNullOrEmpty(npcID))
        {
            npcID = gameObject.name + "_" + transform.position;
        }

        LoadNPCState();

        if (hasChangedState && disableAfterStateChange)
        {
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        CheckPlayerDistance();
    }

    private void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRange;

        if (playerInRange && !wasInRange)
        {
            if (InteractionManager.Instance != null)
            {
                InteractionManager.Instance.RegisterInteraction(this);
                isRegistered = true;
            }
        }
        else if (!playerInRange && wasInRange)
        {
            if (InteractionManager.Instance != null && isRegistered)
            {
                InteractionManager.Instance.UnregisterInteraction(this);
                isRegistered = false;
            }
        }
    }

    private void OnDestroy()
    {
        if (InteractionManager.Instance != null && isRegistered)
        {
            InteractionManager.Instance.UnregisterInteraction(this);
        }
    }

    // IInteractable implementation
    public string GetInteractionText()
    {
        return IsStateConditionMet() ? $"Thank {npcName}" : $"Talk to {npcName}";
    }

    public void Interact()
    {
        StartDialogue();
    }

    public int GetInteractionPriority()
    {
        return interactionPriority;
    }

    private void StartDialogue()
    {
        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.HideInteractionUI();
        }

        if (DialogueManager.Instance != null)
        {
            bool conditionMet = IsStateConditionMet();

            if (conditionMet && !hasChangedState)
            {
                if (inkDialogueFileAfterConditionMet != null)
                {
                    DialogueManager.Instance.StartDialogueWithCallback(
                        inkDialogueFileAfterConditionMet,
                        OnStateConditionDialogueComplete
                    );
                }
                else
                {
                    Debug.LogWarning($"No post-condition dialogue assigned for {npcName}");
                }
            }
            else if (inkDialogueFile != null)
            {
                DialogueManager.Instance.StartDialogueWithState(
                    inkDialogueFile,
                    hasBeenTalkedTo,
                    OnFirstDialogueComplete
                );
            }
        }
        else
        {
            Debug.LogWarning($"Cannot start dialogue for {npcName}: Missing DialogueManager");
        }
    }

    private bool IsStateConditionMet()
    {
        if (string.IsNullOrEmpty(stateConditionKey))
            return false;

        return conditionChecker.IsConditionMet(stateConditionKey);
    }

    private void OnFirstDialogueComplete()
    {
        hasBeenTalkedTo = true;
        SaveNPCState();
    }

    private void OnStateConditionDialogueComplete()
    {
        hasChangedState = true;
        SaveNPCState();

        if (disableAfterStateChange)
        {
            StartCoroutine(DisappearSequence());
        }
    }

    private IEnumerator DisappearSequence()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        if (hideWithFade && fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            float fadeTimer = 0f;

            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeDuration);
                yield return null;
            }
            fadeCanvasGroup.alpha = 1f;
        }

        yield return new WaitForSeconds(1f);

        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            spriteRenderer.enabled = false;

        if (TryGetComponent<Collider2D>(out var collider))
            collider.enabled = false;

        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;

        yield return new WaitForSeconds(0.5f);

        if (hideWithFade && fadeCanvasGroup != null)
        {
            float fadeTimer = 0f;

            while (fadeTimer < fadeDuration)
            {
                fadeTimer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(false);
        }

        if (playerController != null)
            playerController.enabled = true;

        gameObject.SetActive(false);

        Debug.Log($"{npcName} has disappeared after state change!");
    }

    private void SaveNPCState()
    {
        PlayerPrefs.SetInt("NPC_" + npcID + "_talked", hasBeenTalkedTo ? 1 : 0);
        PlayerPrefs.SetInt("NPC_" + npcID + "_changed", hasChangedState ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadNPCState()
    {
        hasBeenTalkedTo = PlayerPrefs.GetInt("NPC_" + npcID + "_talked", 0) == 1;
        hasChangedState = PlayerPrefs.GetInt("NPC_" + npcID + "_changed", 0) == 1;
    }

    public void ResetNPCState()
    {
        hasBeenTalkedTo = false;
        hasChangedState = false;
        SaveNPCState();
        gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}
