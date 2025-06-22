using UnityEngine;

public class NPCDialogue : MonoBehaviour, IInteractable
{
    [Header("Dialogue")]
    [SerializeField] private TextAsset inkDialogueFile;
    [SerializeField] private string npcName = "Villager";
    [SerializeField] private string npcID;

    [Header("Interaction")]
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private int interactionPriority = 1; // Lower priority than doors

    private bool playerInRange = false;
    private Transform player;
    private bool hasBeenTalkedTo = false;
    private bool isRegistered = false;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

        // Generate unique ID if not set
        if (string.IsNullOrEmpty(npcID))
        {
            npcID = gameObject.name + "_" + transform.position.ToString();
        }

        // Load the talked-to state from PlayerPrefs
        LoadNPCState();
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

        if (DialogueManager.Instance != null && inkDialogueFile != null)
        {
            // Start dialogue with callback to mark as talked to when dialogue ends
            DialogueManager.Instance.StartDialogueWithState(
                inkDialogueFile,
                hasBeenTalkedTo,
                OnFirstDialogueComplete
            );
        }
        else
        {
            Debug.LogWarning($"Cannot start dialogue for {npcName}: Missing DialogueManager or Ink file");
        }
    }

    private void OnFirstDialogueComplete()
    {
        // Only called when a first-time dialogue is completed
        hasBeenTalkedTo = true;
        SaveNPCState();
    }

    private void SaveNPCState()
    {
        PlayerPrefs.SetInt("NPC_" + npcID + "_talked", hasBeenTalkedTo ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadNPCState()
    {
        hasBeenTalkedTo = PlayerPrefs.GetInt("NPC_" + npcID + "_talked", 0) == 1;
    }

    // Public method to reset NPC state (useful for testing or game resets)
    public void ResetNPCState()
    {
        hasBeenTalkedTo = false;
        SaveNPCState();
    }

    private void OnDrawGizmosSelected()
    {
        // Draw interaction range in Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}