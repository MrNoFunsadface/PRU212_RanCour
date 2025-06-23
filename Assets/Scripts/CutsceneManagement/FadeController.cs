using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("Fade Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private void Start()
    {
        // Ensure the fade panel starts invisible
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.gameObject.SetActive(false);
        }
    }

    // Public method to get the fade canvas group (for NPCs to use)
    public CanvasGroup GetFadeCanvasGroup()
    {
        return fadeCanvasGroup;
    }
}