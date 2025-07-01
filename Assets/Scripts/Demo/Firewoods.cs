using UnityEngine;

public class Firewoods : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private string stateKey = "Firewoods";
    [SerializeField] private int interactionPriority = 0;

    public string GetInteractionText()
    {
        return "Touch the Firewoods";
    }

    public int GetInteractionPriority()
    {
        return interactionPriority;
    }

    public void Interact()
    {
        // Set the PlayerPref state
        PlayerPrefs.SetInt(stateKey, 1);
        PlayerPrefs.Save();

        Debug.Log($"Firewoods touched! PlayerPrefs key '{stateKey}' set to 1.");
    }
}
