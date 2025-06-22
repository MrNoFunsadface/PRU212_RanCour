using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject interactionPanel;
    [SerializeField] private TextMeshProUGUI interactionText;
    [SerializeField] private TextMeshProUGUI cycleTip; // "Press TAB to cycle"

    [Header("Keys")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private KeyCode cycleKey = KeyCode.Tab;

    private List<IInteractable> availableInteractions = new List<IInteractable>();
    private int currentInteractionIndex = 0;

    public static InteractionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    private void Update()
    {
        if (availableInteractions.Count > 0)
        {
            if (Input.GetKeyDown(interactKey))
            {
                // Only interact with the currently selected interaction
                var currentInteraction = availableInteractions[currentInteractionIndex];
                Debug.Log($"InteractionManager: Interacting with {currentInteraction.GetInteractionText()}");
                currentInteraction.Interact();
            }

            if (Input.GetKeyDown(cycleKey) && availableInteractions.Count > 1)
            {
                CycleInteraction();
            }
        }
    }

    public void RegisterInteraction(IInteractable interaction)
    {
        if (!availableInteractions.Contains(interaction))
        {
            availableInteractions.Add(interaction);

            // Sort by priority (higher priority first)
            availableInteractions = availableInteractions
                .OrderByDescending(i => i.GetInteractionPriority())
                .ToList();

            // Reset index to 0 when new interactions are added
            currentInteractionIndex = 0;

            Debug.Log($"Registered interaction: {interaction.GetInteractionText()} (Priority: {interaction.GetInteractionPriority()})");
            UpdateUI();
        }
    }

    public void UnregisterInteraction(IInteractable interaction)
    {
        if (availableInteractions.Contains(interaction))
        {
            int removedIndex = availableInteractions.IndexOf(interaction);
            availableInteractions.Remove(interaction);

            // Adjust current index if needed
            if (currentInteractionIndex >= availableInteractions.Count)
            {
                currentInteractionIndex = 0;
            }
            else if (removedIndex <= currentInteractionIndex && currentInteractionIndex > 0)
            {
                currentInteractionIndex--;
            }

            Debug.Log($"Unregistered interaction: {interaction.GetInteractionText()}");
            UpdateUI();
        }
    }

    private void CycleInteraction()
    {
        currentInteractionIndex = (currentInteractionIndex + 1) % availableInteractions.Count;
        Debug.Log($"Cycled to interaction {currentInteractionIndex + 1}: {availableInteractions[currentInteractionIndex].GetInteractionText()}");
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (availableInteractions.Count == 0)
        {
            if (interactionPanel != null)
                interactionPanel.SetActive(false);
            return;
        }

        if (interactionPanel != null)
            interactionPanel.SetActive(true);

        var currentInteraction = availableInteractions[currentInteractionIndex];

        if (interactionText != null)
        {
            interactionText.text = $"{currentInteraction.GetInteractionText()}";
        }

        if (cycleTip != null)
        {
            cycleTip.gameObject.SetActive(availableInteractions.Count > 1);
            if (availableInteractions.Count > 1)
            {
                cycleTip.text = $"[TAB] Switch ({currentInteractionIndex + 1}/{availableInteractions.Count})";
            }
        }
    }

    // Nuclear option: Hide UI completely (for dialogue scenes)
    public void HideInteractionUI()
    {
        if (interactionPanel != null)
            interactionPanel.SetActive(false);
    }

    // Show UI again (call this when dialogue ends)
    public void ShowInteractionUI()
    {
        UpdateUI(); // This will show the panel if there are interactions available
    }

    // Debug method to see what's currently registered
    public void LogCurrentInteractions()
    {
        Debug.Log($"Available interactions ({availableInteractions.Count}):");
        for (int i = 0; i < availableInteractions.Count; i++)
        {
            string marker = i == currentInteractionIndex ? " [SELECTED]" : "";
            Debug.Log($"  {i}: {availableInteractions[i].GetInteractionText()} (Priority: {availableInteractions[i].GetInteractionPriority()}){marker}");
        }
    }
}

// Interface that all interactable objects should implement
public interface IInteractable
{
    string GetInteractionText();
    void Interact();
    int GetInteractionPriority(); // Higher = more important
}