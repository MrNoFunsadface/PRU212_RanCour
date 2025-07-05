using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResourceBar : MonoBehaviour
{
    [Header("Resource Bar Elements")]
    [SerializeField] private Image barFill;
    [SerializeField] private int resourceCurrent = 100;
    [SerializeField] private int resourceMax = 100;
    [SerializeField] private bool overkillAllowed;

    [Header("Text Settings")]
    [SerializeField] private DisplayType displayType = DisplayType.Percentage;
    [SerializeField] private TMP_Text resourceTextField;

    private enum DisplayType
    {
        [InspectorName("Long (50|100)")]
        Long,
        [InspectorName("Short (50)")]
        Short,
        [InspectorName("Percentage (50%)")]
        Percentage,
        None
    }

    private void Start()
    {
        UpdateResourceBar();
    }

    public void Initialize(int current, int max)
    {
        resourceCurrent = current;
        resourceMax = max;
        UpdateResourceBar();
    }

    public bool UpdateResourceByAmount(int amount)
    {
        if (!overkillAllowed && resourceCurrent + amount < 0) return false;
        resourceCurrent += amount;
        UpdateResourceBar();

        return true;
    }

    public void UpdateResourceBar()
    {
        if (resourceMax == 0)
        {
            barFill.fillAmount = 0;
            UpdateText(resourceCurrent);
            return;
        }
        float fillAmount = (float)resourceCurrent / resourceMax;
        barFill.fillAmount = fillAmount;
        UpdateText(resourceCurrent);
    }

    private void UpdateText(int currentCost)
    {
        switch (displayType)
        {
            case DisplayType.Long:
                resourceTextField.SetText($"{currentCost}/{resourceMax}");
                break;
            case DisplayType.Short:
                resourceTextField.SetText($"{currentCost}");
                break;
            case DisplayType.Percentage:
                resourceTextField.SetText($"{(currentCost * 100 / resourceMax)}%");
                break;
            case DisplayType.None:
                resourceTextField.SetText(string.Empty);
                break;
        }
    }

    public void ResetToMax()
    {
        resourceCurrent = resourceMax;
        UpdateResourceBar();
    }
}
