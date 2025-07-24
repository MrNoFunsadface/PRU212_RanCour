using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashEffect : MonoBehaviour
{
    private Image whitePanel;
    public float flashDuration = 0.3f;

    private void Awake()
    {
        whitePanel = GetComponent<Image>();
        if (whitePanel != null)
        {
            // Ensure it's fully transparent at start
            Color initialColor = whitePanel.color;
            initialColor.a = 0f;
            whitePanel.color = initialColor;
        }
    }

    private void OnEnable()
    {
        if (whitePanel != null)
            StartCoroutine(Flash());
        else
            Debug.LogError("FlashEffect: Image component not found on this GameObject.");
    }

    private IEnumerator Flash()
    {
        float halfDuration = flashDuration / 2f;
        float t = 0f;
        Color color = whitePanel.color;

        // Fade in
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, t / halfDuration);
            whitePanel.color = color;
            yield return null;
        }

        // Fade out
        t = 0f;
        while (t < halfDuration)
        {
            t += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, t / halfDuration);
            whitePanel.color = color;
            yield return null;
        }

        // Optional: disable again after flashing
        gameObject.SetActive(false);
    }
}
