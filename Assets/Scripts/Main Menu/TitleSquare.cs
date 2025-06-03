using UnityEngine;
using UnityEngine.UI;

public class TitleSquare : MonoBehaviour
{
    private Image titleSquareImage1;
    private Image titleSquareImage2;

    void Start()
    {
        titleSquareImage1 = transform.Find("Square 1").GetComponent<Image>();
        titleSquareImage2 = transform.Find("Square 2").GetComponent<Image>();
    }
    void Update()
    {
        if (titleSquareImage1 != null && titleSquareImage2 != null)
        {
            titleSquareImage1.transform.Rotate(0f, 0f, 10f * Time.deltaTime);
            titleSquareImage2.transform.Rotate(0f, 0f, 10f * Time.deltaTime);
        }
    }
}
