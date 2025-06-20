using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    public MonologueData monologueData;
    public GameObject interactPrompt;

    [SerializeField]
    private EButton eButton;

    private bool playerInRange = false;

    private void Start()
    {
        interactPrompt.SetActive(false);
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (monologueData != null)
            {
                MonologueSystem.Instance.ShowMonologue(monologueData.monologueText);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
            eButton.Show();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
            eButton.Hide();
        }
    }
}
