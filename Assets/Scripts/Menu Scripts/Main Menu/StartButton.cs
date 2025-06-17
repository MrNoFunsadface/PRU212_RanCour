using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image startButtonImage;
    private Image startButtonOutlineImage;

    private Color startButtonOriginalColor;
    private Color startButtonOutlineOriginalColor;

    public Color startButtonTempColor;
    public Color startButtonOutlineTempColor;

    public bool isPointerOverButton = false;
    public bool isStartButtonClicked = false;

    void Start()
    {
        startButtonImage = transform.parent.GetComponent<Image>();

        if (startButtonImage != null)
        {
            startButtonOutlineImage = startButtonImage.transform.Find("Start button outline").GetComponent<Image>();
        }

        if (startButtonImage != null)
        {
            startButtonOriginalColor = startButtonImage.color;
        }

        if (startButtonOutlineImage != null)
        {
            startButtonOutlineOriginalColor = startButtonOutlineImage.color;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (isStartButtonClicked)
            return; // Prevent action if button is clicked

        isPointerOverButton = true;
        StartCoroutine(FlashButtonColor());
        StartCoroutine(FlashButtonOutlineColor());
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (isStartButtonClicked)
            return; // Prevent action if button is clicked

        isPointerOverButton = false;
        StopAllCoroutines();
        ResetButtonColor();
    }

    private IEnumerator FlashButtonColor()
    {
        if (startButtonImage != null)
        {
            while (isPointerOverButton)
            {
                startButtonTempColor = startButtonOriginalColor;

                yield return new WaitForSeconds(0.1f);
                while (startButtonTempColor.r < 1f && isPointerOverButton)
                {
                    startButtonTempColor.r += 0.1f;
                    startButtonImage.color = startButtonTempColor;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.1f);
                while (startButtonTempColor.r > startButtonOriginalColor.r && isPointerOverButton)
                {
                    startButtonTempColor.r -= 0.1f;
                    startButtonImage.color = startButtonTempColor;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private IEnumerator FlashButtonOutlineColor()
    {
        if (startButtonOutlineImage != null)
        {
            while (isPointerOverButton)
            {
                startButtonOutlineTempColor = startButtonOutlineOriginalColor;

                yield return new WaitForSeconds(0.1f);
                while (startButtonOutlineTempColor.r < 1f && isPointerOverButton)
                {
                    startButtonOutlineTempColor.r += 0.1f;
                    startButtonOutlineImage.color = startButtonOutlineTempColor;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.1f);
                while (startButtonOutlineTempColor.r > startButtonOutlineOriginalColor.r && isPointerOverButton)
                {
                    startButtonOutlineTempColor.r -= 0.1f;
                    startButtonOutlineImage.color = startButtonOutlineTempColor;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private void ResetButtonColor()
    {
        if (startButtonImage != null)
        {
            startButtonImage.color = startButtonOriginalColor;
        }

        if (startButtonOutlineImage != null)
        {
            startButtonOutlineImage.color = startButtonOutlineOriginalColor;
        }
    }

    public void OnStartButtonClick()
    {
        StopAllCoroutines();
        isStartButtonClicked = true;

        startButtonTempColor = startButtonOriginalColor;
        startButtonOutlineTempColor = startButtonOutlineOriginalColor;

        startButtonTempColor.r = 1f;
        startButtonOutlineTempColor.r = 1f;

        startButtonImage.color = startButtonTempColor;
        startButtonOutlineImage.color = startButtonOutlineTempColor;

        StartCoroutine(ChangeStartButtonColour());
        StartCoroutine(ChangeStartButtonOutlineColour());

        StartCoroutine(ChangeScene());
    }

    private IEnumerator ChangeScene()
    {
        PlayerPrefs.DeleteAll(); // Clear PlayerPrefs for a fresh start
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Scene0");
    }

    private IEnumerator ChangeStartButtonColour()
    {
        startButtonTempColor = startButtonImage.color;
        yield return new WaitForSeconds(0.1f);
        while (startButtonTempColor.r > startButtonOriginalColor.r && isPointerOverButton)
        {
            startButtonTempColor.r -= 0.1f;
            startButtonImage.color = startButtonTempColor;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private IEnumerator ChangeStartButtonOutlineColour()
    {
        startButtonOutlineTempColor = startButtonOutlineImage.color;
        yield return new WaitForSeconds(0.1f);
        while (startButtonOutlineTempColor.r > startButtonOutlineOriginalColor.r && isPointerOverButton)
        {
            startButtonOutlineTempColor.r -= 0.1f;
            startButtonOutlineImage.color = startButtonOutlineTempColor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}