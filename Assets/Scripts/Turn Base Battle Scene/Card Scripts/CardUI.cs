using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI costText;

    public void Setup(Card data)
    {
        if (data == null) return;
        if (cardNameText != null) cardNameText.text = data.cardName;
        if (artworkImage != null && data.artwork != null) artworkImage.sprite = data.artwork;
        if (costText != null) costText.text = data.cost.ToString();
    }
}
