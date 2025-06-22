using Scripts.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Interactable
{
    public class PrisonDoor : MonoBehaviour, IInteractable
    {
        private bool playerInRange = false;
        private bool isRegistered = false;
        private Collider2D gateCollider;
        private Animator mainGateAnimator;
        private string doorName;

        [SerializeField] private EButton eButton;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private int interactionPriority = 5; // Higher priority than NPCs

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInRange = true;

                // Register with interaction manager if it exists, otherwise use old system
                if (InteractionManager.Instance != null)
                {
                    InteractionManager.Instance.RegisterInteraction(this);
                    isRegistered = true;
                }
                else
                {
                    eButton.Show();
                }
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerInRange = false;

                // Unregister from interaction manager if it exists, otherwise use old system
                if (InteractionManager.Instance != null && isRegistered)
                {
                    InteractionManager.Instance.UnregisterInteraction(this);
                    isRegistered = false;
                }
                else
                {
                    eButton.Hide();
                }
            }
        }

        private void OnDestroy()
        {
            // Clean up registration
            if (InteractionManager.Instance != null && isRegistered)
            {
                InteractionManager.Instance.UnregisterInteraction(this);
            }
        }

        // IInteractable implementation
        public string GetInteractionText()
        {
            if (inventoryData.CheckItemByName("MasterKey"))
            {
                return $"Unlock {doorName}";
            }
            return $"Locked {doorName} (Need MasterKey)";
        }

        public void Interact()
        {
            TryOpenGate();
        }

        public int GetInteractionPriority()
        {
            return interactionPriority;
        }

        private void TryOpenGate()
        {
            Debug.Log("PrisonDoor.TryOpenGate() called - THIS SHOULD NOT HAPPEN when NPC is selected!");

            if (inventoryData.CheckItemByName("MasterKey"))
            {
                Debug.Log("You have the key. Opening the gate...");
                StartCoroutine(OpenGate());
                PlayerPrefs.SetInt(doorName, 1);
            }
            else
            {
                Debug.Log($"You need a key to open the {doorName}.");
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
                Debug.LogError($"{doorName} script requires a Collider2D component on the GameObject.");
            }

            mainGateAnimator = GetComponent<Animator>();
            if (mainGateAnimator == null)
            {
                Debug.LogError($"{doorName} script requires an Animator component on the GameObject.");
            }

            doorName = gameObject.name;
            int gateOpened = PlayerPrefs.GetInt(doorName, 0);

            if (gateOpened == 1)
            {
                gateCollider.enabled = false;
                mainGateAnimator.Play("GateStayOpenedAnim", 0, 0f);
                Debug.Log($"{doorName} has already been opened.");
                return;
            }
            else
            {
                gateCollider.enabled = true;
                eButton.Hide();
                Debug.Log($"{doorName} is available to open.");
            }
        }

        private void Update()
        {
            // Only handle direct input if InteractionManager is not present (backward compatibility)
            if (InteractionManager.Instance == null && playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
            {
                TryOpenGate();
            }
            // If InteractionManager exists, it will call our Interact() method when appropriate
            // NO DIRECT INPUT HANDLING when InteractionManager is present
        }
    }
}