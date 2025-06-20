using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private TextAsset inkDialogueFile;
    [SerializeField] private string npcName = "Villager";

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private float interactionRange = 2f;
    [SerializeField] private GameObject interactionPrompt;

    private bool playerInRange = false;
    private Transform player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().transform;

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        CheckPlayerDistance();

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            StartDialogue();
        }
    }

    private void CheckPlayerDistance()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        bool wasInRange = playerInRange;
        playerInRange = distance <= interactionRange;

        if (playerInRange && !wasInRange)
        {
            ShowInteractionPrompt();
        }
        else if (!playerInRange && wasInRange)
        {
            HideInteractionPrompt();
        }
    }

    private void ShowInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(true);
        }
    }

    private void HideInteractionPrompt()
    {
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    private void StartDialogue()
    {
        if (DialogueManager.Instance != null && inkDialogueFile != null)
        {
            HideInteractionPrompt();
            DialogueManager.Instance.StartDialogue(inkDialogueFile);
        }
        else
        {
            Debug.LogWarning($"Cannot start dialogue for {npcName}: Missing DialogueManager or Ink file");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw interaction range in Scene view
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}