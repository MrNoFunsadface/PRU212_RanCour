using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image artworkImage;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI costText;

    public void Setup(Card data)
    {
        if (data == null) return;
        if (cardNameText != null) cardNameText.text = data.cardName;
        if (descriptionText != null) descriptionText.text = data.description;
        if (artworkImage != null && data.artwork != null) artworkImage.sprite = data.artwork;
        if (costText != null) costText.text = data.cost.ToString();
    }

    public void OnCardClick()
    {
        // Flip the card to the back for extra information, description, etc.
    }
}
