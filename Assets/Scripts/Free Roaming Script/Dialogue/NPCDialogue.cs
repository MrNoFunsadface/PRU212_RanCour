using System.Collections;
using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private TextAsset inkDialogueFile;
    [SerializeField] private TextAsset inkDialogueFileFreed; // New dialogue after being freed
    [SerializeField] private string npcName = "Villager";
    [SerializeField] private string npcID;
    [SerializeField] private string associatedDoorName; // Name of the prison door that frees this NPC

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private int interactionPriority = 1; // Lower priority than doors

    [Header("Escape Settings")]
    [SerializeField] private float fadeToBlackDuration = 2f;
    [SerializeField] private CanvasGroup fadeCanvasGroup; // Assign a black panel with CanvasGroup for fading

    private bool playerInRange = false;
    private Transform player;
    private bool hasBeenTalkedTo = false;
    private bool hasBeenFreed = false;
    private bool isRegistered = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

        // Generate unique ID if not set
        if (string.IsNullOrEmpty(npcID))
        {
            npcID = gameObject.name + "_" + transform.position.ToString();
        }

        // Load the NPC state from PlayerPrefs
        LoadNPCState();

        // If NPC has been freed, disable this gameObject
        if (hasBeenFreed)
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
            // Register with interaction manager
            if (InteractionManager.Instance != null)
            {
                InteractionManager.Instance.RegisterInteraction(this);
                isRegistered = true;
            }
        }
        else if (!playerInRange && wasInRange)
        {
            // Unregister from interaction manager
            if (InteractionManager.Instance != null && isRegistered)
            {
                InteractionManager.Instance.UnregisterInteraction(this);
                isRegistered = false;
            }
        }
    }

    private void OnDestroy()
    {
        // Clean up registration
        if (InteractionManager.Instance != null && isRegistered)
        {
            InteractionManager.Instance.UnregisterInteraction(this);
        }
    }

    // IInteractable implementation
    public string GetInteractionText()
    {
        if (IsNPCFreed())
        {
            return $"Thank {npcName}";
        }
        return $"Talk to {npcName}";
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
        // NUCLEAR OPTION: Hide the interaction panel completely when dialogue starts
        if (InteractionManager.Instance != null)
        {
            InteractionManager.Instance.HideInteractionUI();
        }

        if (DialogueManager.Instance != null)
        {
            // Check if NPC has been freed by checking if the associated door has been opened
            bool npcFreed = IsNPCFreed();

            if (npcFreed && !hasBeenFreed)
            {
                // This is the first time talking to the NPC after being freed
                if (inkDialogueFileFreed != null)
                {
                    // Use the simple StartDialogue method for freed dialogue (no state variables needed)
                    DialogueManager.Instance.StartDialogueWithCallback(
                        inkDialogueFileFreed,
                        OnFreedDialogueComplete
                    );
                }
                else
                {
                    Debug.LogWarning($"No freed dialogue file assigned for {npcName}");
                }
            }
            else if (inkDialogueFile != null)
            {
                // Normal dialogue (NPC still imprisoned)
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

    private bool IsNPCFreed()
    {
        if (string.IsNullOrEmpty(associatedDoorName))
        {
            return false;
        }

        // Check if the associated prison door has been opened
        return PlayerPrefs.GetInt(associatedDoorName, 0) == 1;
    }

    private void OnFirstDialogueComplete()
    {
        // Only called when a first-time dialogue is completed (normal imprisoned dialogue)
        hasBeenTalkedTo = true;
        SaveNPCState();
    }

    private void OnFreedDialogueComplete()
    {
        // Called when the "thank you" dialogue after being freed is completed
        hasBeenFreed = true;
        SaveNPCState();

        // Start the escape sequence
        StartCoroutine(EscapeSequence());
    }

    private IEnumerator EscapeSequence()
    {
        var playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
            playerController.enabled = false;

        // Fade to black
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.gameObject.SetActive(true);
            float fadeTimer = 0f;

            while (fadeTimer < fadeToBlackDuration)
            {
                fadeTimer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, fadeTimer / fadeToBlackDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = 1f;
        }

        // Wait in black screen for a moment (simulate disappearance)
        yield return new WaitForSeconds(1f);

        // Hide the NPC (but NOT disable the GameObject)
        if (TryGetComponent<SpriteRenderer>(out var spriteRenderer))
            spriteRenderer.enabled = false;

        if (TryGetComponent<Collider2D>(out var collider))
            collider.enabled = false;

        // Optionally hide animations or effects
        var animator = GetComponent<Animator>();
        if (animator != null)
            animator.enabled = false;

        // Wait just a bit before fading in (optional for pacing)
        yield return new WaitForSeconds(0.5f);

        // Fade back in
        if (fadeCanvasGroup != null)
        {
            float fadeTimer = 0f;

            while (fadeTimer < fadeToBlackDuration)
            {
                fadeTimer += Time.deltaTime;
                fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeToBlackDuration);
                yield return null;
            }

            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(false);
        }

        // Re-enable player control
        if (playerController != null)
            playerController.enabled = true;

        // Finally disable this GameObject (after fade-in)
        gameObject.SetActive(false);

        Debug.Log($"{npcName} has escaped!");
    }



    private void SaveNPCState()
    {
        PlayerPrefs.SetInt("NPC_" + npcID + "_talked", hasBeenTalkedTo ? 1 : 0);
        PlayerPrefs.SetInt("NPC_" + npcID + "_freed", hasBeenFreed ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadNPCState()
    {
        hasBeenTalkedTo = PlayerPrefs.GetInt("NPC_" + npcID + "_talked", 0) == 1;
        hasBeenFreed = PlayerPrefs.GetInt("NPC_" + npcID + "_freed", 0) == 1;
    }

    // Public method to reset NPC state (useful for testing or game resets)
    public void ResetNPCState()
    {
        hasBeenTalkedTo = false;
        hasBeenFreed = false;
        SaveNPCState();
        gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw interaction range in Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}