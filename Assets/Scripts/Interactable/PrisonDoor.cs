using Scripts.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Interactable
{
    public class PrisonDoor : MonoBehaviour
    {
        private bool playerInRange = false;
        private Collider2D gateCollider;
        private Animator mainGateAnimator;
        private string doorName;

        [SerializeField]
        private EButton eButton;

        [SerializeField]
        private InventorySO inventoryData;

        [SerializeField]
        private PlayerController playerController;

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
            playerController.setSpeed(0f);
            Debug.Log("Opening gate...");
            yield return new WaitForSeconds(2f);
            Debug.Log("Gate opened successfully.");
            playerController.setSpeed(6f);
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

            doorName = gameObject.name;

            int gateOpened = PlayerPrefs.GetInt(doorName, 0);
            if (gateOpened == 1)
            {
                gateCollider.enabled = false;
                mainGateAnimator.Play("GateStayOpenedAnim", 0, 0f);
                Debug.Log("Main gate has already been opened.");
                return;
            }
            else
            {
                gateCollider.enabled = true;
                eButton.Hide();
                Debug.Log("Main gate is available to open.");
            }
        }

        private void Update()
        {
            if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
            {
                if (inventoryData.CheckItemByName("MasterKey"))
                {
                    Debug.Log("You have the key. Opening the gate...");
                    StartCoroutine(OpenGate());
                    PlayerPrefs.SetInt(doorName, 1);
                }
                else Debug.Log("You need a key to open the gate.");
            }
        }
    }
}