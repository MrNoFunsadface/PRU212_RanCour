using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Image quitButtonImage;
    private Image quitButtonOutlineImage;

    private Color quitButtonOriginalColor;
    private Color quitButtonOutlineOriginalColor;

    public Color quitButtonTempColor;
    public Color quitButtonOutlineTempColor;

    public bool isPointerOverButton = false;
    public bool isQuitButtonClicked = false;

    void Start()
    {
        quitButtonImage = transform.parent.GetComponent<Image>();

        if (quitButtonImage != null)
        {
            quitButtonOutlineImage = quitButtonImage.transform.Find("Quit button outline").GetComponent<Image>();
        }

        if (quitButtonImage != null)
        {
            quitButtonOriginalColor = quitButtonImage.color;
        }

        if (quitButtonOutlineImage != null)
        {
            quitButtonOutlineOriginalColor = quitButtonOutlineImage.color;
        }
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isPointerOverButton = true;
        StartCoroutine(FlashButtonColor());
        StartCoroutine(FlashButtonOutlineColor());
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (isQuitButtonClicked)
            return;
        isPointerOverButton = false;
        StopAllCoroutines();
        ResetButtonColor();
    }

    private IEnumerator FlashButtonColor()
    {
        if (quitButtonImage != null)
        {
            while (isPointerOverButton)
            {
                quitButtonTempColor = quitButtonOriginalColor;

                yield return new WaitForSeconds(0.1f);
                while (quitButtonTempColor.r < 1f && isPointerOverButton)
                {
                    quitButtonTempColor.r += 0.1f;
                    quitButtonImage.color = quitButtonTempColor;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.1f);
                while (quitButtonTempColor.r > quitButtonOriginalColor.r && isPointerOverButton)
                {
                    quitButtonTempColor.r -= 0.1f;
                    quitButtonImage.color = quitButtonTempColor;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private IEnumerator FlashButtonOutlineColor()
    {
        if (quitButtonOutlineImage != null)
        {
            while (isPointerOverButton)
            {
                quitButtonOutlineTempColor = quitButtonOutlineOriginalColor;

                yield return new WaitForSeconds(0.1f);
                while (quitButtonOutlineTempColor.r < 1f && isPointerOverButton)
                {
                    quitButtonOutlineTempColor.r += 0.1f;
                    quitButtonOutlineImage.color = quitButtonOutlineTempColor;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.1f);
                while (quitButtonOutlineTempColor.r > quitButtonOutlineOriginalColor.r && isPointerOverButton)
                {
                    quitButtonOutlineTempColor.r -= 0.1f;
                    quitButtonOutlineImage.color = quitButtonOutlineTempColor;
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }
    }

    private void ResetButtonColor()
    {
        if (quitButtonImage != null)
        {
            quitButtonImage.color = quitButtonOriginalColor;
        }

        if (quitButtonOutlineImage != null)
        {
            quitButtonOutlineImage.color = quitButtonOutlineOriginalColor;
        }
    }

    public void OnQuitButtonClick()
    {
        isQuitButtonClicked = true;
        StopAllCoroutines();

        if (quitButtonImage != null && quitButtonOutlineImage != null)
        {
            quitButtonTempColor = quitButtonOriginalColor;
            quitButtonOutlineTempColor = quitButtonOutlineOriginalColor;

            quitButtonTempColor.r = 1f;
            quitButtonOutlineTempColor.r = 1f;

            quitButtonOutlineImage.color = quitButtonOutlineTempColor;
            quitButtonImage.color = quitButtonTempColor;

            StartCoroutine(ChangeButtonColour());
        }

        isQuitButtonClicked = false;
        Application.Quit();
    }

    private IEnumerator ChangeButtonColour()
    {
        yield return new WaitForSeconds(0.1f);
        isQuitButtonClicked = false;

        if (quitButtonImage != null && quitButtonOutlineImage != null)
        {
            yield return new WaitForSeconds(0.1f);
            while (quitButtonTempColor.r > quitButtonOriginalColor.r)
            {
                quitButtonTempColor.r -= 0.1f;
                quitButtonOutlineTempColor.r -= 0.1f;

                quitButtonImage.color = quitButtonTempColor;
                quitButtonOutlineImage.color = quitButtonOutlineTempColor;

                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerable ChangeButtonOutlineColour()
    {
        yield return new WaitForSeconds(0.1f);
        isQuitButtonClicked = false;

        if (quitButtonOutlineImage != null)
        {
            yield return new WaitForSeconds(0.1f);
            while (quitButtonOutlineTempColor.r > quitButtonOutlineOriginalColor.r)
            {
                quitButtonOutlineTempColor.r -= 0.1f;
                quitButtonOutlineImage.color = quitButtonOutlineTempColor;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
