//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using Cinemachine;

//public class CutsceneManager : MonoBehaviour
//{
//    [Header("Scene References")]
//    [SerializeField] private string gameSceneName = "Scene0";

//    [Header("Characters")]
//    [SerializeField] private Transform player;
//    [SerializeField] private Transform mysteriousStranger;
//    [SerializeField] private Animator playerAnimator;
//    [SerializeField] private Animator strangerAnimator;

//    [Header("Cameras")]
//    [SerializeField] private CinemachineVirtualCamera cellCamera;
//    [SerializeField] private CinemachineVirtualCamera playerWakeUpCamera;

//    [Header("Cutscene Settings")]
//    [SerializeField] private float strangerWalkDuration = 3f;
//    [SerializeField] private float pauseBetweenCutscenes = 1f;
//    [SerializeField] private float dialogueDelay = 2f;

//    [Header("Movement")]
//    [SerializeField] private Transform strangerExitPoint;
//    [SerializeField] private float strangerWalkSpeed = 2f;

//    [Header("UI")]
//    [SerializeField] private GameObject fadePanel;
//    [SerializeField] private float fadeTime = 1f;

//    private bool cutsceneActive = false;

//    private void Start()
//    {
//        // Check if we should play the intro cutscene
//        if (PlayerPrefs.GetInt("PlayIntroCutscene", 0) == 1)
//        {
//            PlayerPrefs.DeleteKey("PlayIntroCutscene");
//            StartCoroutine(PlayIntroCutscene());
//        }
//        else
//        {
//            // Skip to game if not coming from main menu
//            LoadGameScene();
//        }
//    }

//    private IEnumerator PlayIntroCutscene()
//    {
//        cutsceneActive = true;

//        // Disable player control
//        if (player.GetComponent<PlayerController>() != null)
//        {
//            player.GetComponent<PlayerController>().enabled = false;
//        }

//        // Fade in from black
//        yield return StartCoroutine(FadeFromBlack());

//        // Play stranger cutscene
//        yield return StartCoroutine(StrangerCutscene());

//        // Pause between cutscenes
//        yield return new WaitForSeconds(pauseBetweenCutscenes);

//        // Play wake up cutscene
//        yield return StartCoroutine(WakeUpCutscene());

//        // Wait before dialogue
//        yield return new WaitForSeconds(dialogueDelay);

//        // Play self dialogue
//        yield return StartCoroutine(SelfDialogue());

//        // Transition to game
//        yield return StartCoroutine(TransitionToGame());
//    }

//    private IEnumerator StrangerCutscene()
//    {
//        // Set camera to cell view
//        cellCamera.Priority = 10;
//        playerWakeUpCamera.Priority = 5;

//        // Player should be lying down (set initial animation state)
//        if (playerAnimator != null)
//        {
//            playerAnimator.SetBool("isLyingDown", true);
//        }

//        // Wait a moment for dramatic effect
//        yield return new WaitForSeconds(1f);

//        // Stranger starts walking away
//        if (mysteriousStranger != null && strangerExitPoint != null)
//        {
//            Vector3 startPosition = mysteriousStranger.position;
//            Vector3 endPosition = strangerExitPoint.position;

//            // Set walking animation
//            if (strangerAnimator != null)
//            {
//                strangerAnimator.SetBool("isWalking", true);
//            }

//            // Face the correct direction
//            Vector3 direction = (endPosition - startPosition).normalized;
//            if (direction.x < 0)
//            {
//                mysteriousStranger.GetComponent<SpriteRenderer>().flipX = true;
//            }

//            // Move stranger to exit point
//            float elapsedTime = 0f;
//            while (elapsedTime < strangerWalkDuration)
//            {
//                mysteriousStranger.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / strangerWalkDuration);
//                elapsedTime += Time.deltaTime;
//                yield return null;
//            }

//            // Stop walking animation
//            if (strangerAnimator != null)
//            {
//                strangerAnimator.SetBool("isWalking", false);
//            }

//            // Deactivate stranger
//            mysteriousStranger.gameObject.SetActive(false);
//        }
//    }

//    private IEnumerator WakeUpCutscene()
//    {
//        // Switch to player wake up camera
//        cellCamera.Priority = 5;
//        playerWakeUpCamera.Priority = 10;

//        // Wait a moment
//        yield return new WaitForSeconds(0.5f);

//        // Player waking up animation
//        if (playerAnimator != null)
//        {
//            playerAnimator.SetBool("isLyingDown", false);
//            playerAnimator.SetTrigger("wakeUp");
//        }

//        // Wait for wake up animation to complete
//        yield return new WaitForSeconds(2f);
//    }

//    private IEnumerator SelfDialogue()
//    {
//        // Create temporary dialogue for player's self-talk
//        if (DialogueManager.Instance != null)
//        {
//            // You can either create a separate Ink file for this or handle it directly
//            StartSelfDialogue();

//            // Wait for dialogue to complete
//            while (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive())
//            {
//                yield return null;
//            }
//        }
//    }

//    private void StartSelfDialogue()
//    {
//        // Create a simple self-dialogue
//        // You should create a separate Ink file called "PlayerSelfDialogue.ink" with content like:
//        /*
//        Where... where am I?
//        * [Look around the cell]
//            This looks like some kind of prison cell. How did I get here?
//            * * [Try to remember]
//                I can't remember anything... What happened to me?
//                -> END
//            * * [Focus on getting out]
//                I need to find a way out of here.
//                -> END
//        * [Check yourself for injuries]
//            I seem to be okay, just a bit sore. But why can't I remember anything?
//            -> END
//        */

//        // Load and start the self-dialogue
//        TextAsset selfDialogueInk = Resources.Load<TextAsset>("PlayerSelfDialogue");
//        if (selfDialogueInk != null && DialogueManager.Instance != null)
//        {
//            DialogueManager.Instance.StartDialogue(selfDialogueInk);
//        }
//    }

//    private IEnumerator TransitionToGame()
//    {
//        // Fade to black
//        yield return StartCoroutine(FadeToBlack());

//        // Load the main game scene
//        LoadGameScene();
//    }

//    private void LoadGameScene()
//    {
//        // Set up game manager state if needed
//        if (GameManager.Instance != null)
//        {
//            GameManager.Instance.useCustomSpawnPosition = true;
//            GameManager.Instance.customSpawnPosition = player.position;
//        }

//        SceneManager.LoadScene(gameSceneName);
//    }

//    private IEnumerator FadeFromBlack()
//    {
//        if (fadePanel != null)
//        {
//            CanvasGroup canvasGroup = fadePanel.GetComponent<CanvasGroup>();
//            if (canvasGroup == null)
//            {
//                canvasGroup = fadePanel.AddComponent<CanvasGroup>();
//            }

//            canvasGroup.alpha = 1f;
//            fadePanel.SetActive(true);

//            float elapsedTime = 0f;
//            while (elapsedTime < fadeTime)
//            {
//                canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeTime);
//                elapsedTime += Time.deltaTime;
//                yield return null;
//            }

//            canvasGroup.alpha = 0f;
//            fadePanel.SetActive(false);
//        }
//    }

//    private IEnumerator FadeToBlack()
//    {
//        if (fadePanel != null)
//        {
//            CanvasGroup canvasGroup = fadePanel.GetComponent<CanvasGroup>();
//            if (canvasGroup == null)
//            {
//                canvasGroup = fadePanel.AddComponent<CanvasGroup>();
//            }

//            fadePanel.SetActive(true);

//            float elapsedTime = 0f;
//            while (elapsedTime < fadeTime)
//            {
//                canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeTime);
//                elapsedTime += Time.deltaTime;
//                yield return null;
//            }
//            canvasGroup.alpha = 1f;
//        }
//    }
//}