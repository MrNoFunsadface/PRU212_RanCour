using TMPro;
using UnityEngine;

public class PlayerCost : MonoBehaviour
{
    public static PlayerCost Instance { get; private set; }
    [SerializeField] private int maxCost;
    [SerializeField] private TextMeshProUGUI playerCostText;
    [SerializeField] private RectTransform emptyFillRT;

    public int CurrentCost;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        ResetCost();
    }

    public void ResetCost()
    {
        CurrentCost = maxCost;
        ChangePlayerCostText(CurrentCost);
        SetEmptyFillRT();
    }

    private void ChangePlayerCostText(int currentCost)
    {
        playerCostText.text = currentCost + "/" + maxCost;
    }

    public bool UpdateCost(int cost)
    {

        if (CurrentCost - cost < 0)
        {
            BattleLogScript.Instance.LogBattleEvent("You need more mana to use this card!");
            BattleLogScript.Instance.UpdateDisplayer();
            return false;
        }
        else CurrentCost -= cost;

        ChangePlayerCostText(CurrentCost);
        SetEmptyFillRT();
        return true;
    }

    private void SetEmptyFillRT()
    {
        float maxHeight = 400f; // full bar height
        float used = maxCost - CurrentCost;
        float height = maxHeight * used / maxCost;
        emptyFillRT.sizeDelta = new Vector2(emptyFillRT.sizeDelta.x, height);
    }
}
