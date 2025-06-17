using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    public MonologueData monologueData;
    public GameObject interactPrompt;

    private bool playerNearby = false;

    private void Start()
    {
        interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (monologueData != null)
            {
                MonologueSystem.Instance.ShowMonologue(monologueData.monologueText);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(true);
            playerNearby = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            interactPrompt.SetActive(false);
            playerNearby = false;
        }
    }
}
