using TMPro;
using UnityEngine;

public class TitleName : MonoBehaviour
{
    private TextMeshProUGUI titleText1;
    private TextMeshProUGUI titleText2;
    private TextMeshProUGUI titleText3;

    [SerializeField] private float tiltAngle;
    [SerializeField] private float tiltSpeed;

    void Start()
    {
        // Use TextMeshProUGUI instead of Text
        titleText1 = transform.Find("Title (black)").GetComponent<TextMeshProUGUI>();
        titleText2 = transform.Find("Title (purple)").GetComponent<TextMeshProUGUI>();
        titleText3 = transform.Find("Title (cyan)").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        // Calculate the tilt angle using Mathf.Sin
        float tilt = Mathf.Sin(Time.time * tiltSpeed) * tiltAngle;

        if (titleText1 != null)
            titleText1.transform.rotation = Quaternion.Euler(0f, 0f, tilt);
        if (titleText2 != null)
            titleText2.transform.rotation = Quaternion.Euler(0f, 0f, tilt);
        if (titleText3 != null)
            titleText3.transform.rotation = Quaternion.Euler(0f, 0f, tilt);
    }
}
