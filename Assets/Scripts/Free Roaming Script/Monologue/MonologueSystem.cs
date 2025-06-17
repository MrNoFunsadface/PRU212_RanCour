using UnityEngine;
using TMPro;

public class MonologueSystem : MonoBehaviour
{
    public static MonologueSystem Instance;

    public GameObject panel;
    public TMP_Text monologueText;

    private bool isShowing = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        panel.SetActive(false);
    }

    public void ShowMonologue(string text)
    {
        panel.SetActive(true);
        monologueText.text = text;
        isShowing = true;
        Time.timeScale = 0f; // Pause game (optional)
    }

    public void HideMonologue()
    {
        panel.SetActive(false);
        isShowing = false;
        Time.timeScale = 1f;
    }

    private void Update()
    {
        if (isShowing && Input.GetKeyDown(KeyCode.Space))
        {
            HideMonologue();
        }
    }
}
