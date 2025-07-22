using Scripts.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainGate : MonoBehaviour
{
    private bool playerInRange = false;
    private Collider2D gateCollider;
    private Animator mainGateAnimator;
    private PlayerController playerController;

    [SerializeField]
    private EButton eButton;

    [SerializeField]
    private InventorySO inventoryData;

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

    private IEnumerator OpenGate()
    {
        gateCollider.enabled = false;
        mainGateAnimator.SetBool("OpenGate", true);
        playerController.SetSpeed(0f);
        Debug.Log("Opening gate...");
        yield return new WaitForSeconds(2f);
        Debug.Log("Gate opened successfully.");
        playerController.SetSpeed(6f);
    }

    private void Start()
    {
        gateCollider = GetComponent<Collider2D>();
        if (gateCollider == null)
        {
            Debug.LogError("MainGate script requires a Collider2D component on the GameObject.");
        }

        mainGateAnimator = GetComponent<Animator>();
        if (mainGateAnimator == null)
        {
            Debug.LogError("MainGate script requires an Animator component on the GameObject.");
        }

        int gateOpened = PlayerPrefs.GetInt("MainGate_Opened", 0);
        if (gateOpened == 1)
        {
            gateCollider.enabled = false;
            mainGateAnimator.Play("GateStayOpenedAnim", 0, 0f);
            return;
        }
        else
        {
            gateCollider.enabled = true;
            eButton.Hide();
        }

        playerController = FindFirstObjectByType<PlayerController>();
    }

    private void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (inventoryData.CheckItemByName("Key"))
            {
                Debug.Log("You have the key. Opening the gate...");
                StartCoroutine(OpenGate());
                PlayerPrefs.SetInt("MainGate_Opened", 1);
            }
            else Debug.Log("You need a key to open the gate.");
        }
    }
}
