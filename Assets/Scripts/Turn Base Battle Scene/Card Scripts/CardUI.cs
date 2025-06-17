using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private Image Image;
    [SerializeField] private TextMeshProUGUI costText;

    private RawImage artworkImage;

    public void Setup(Card data)
    {
        artworkImage = Image.GetComponentInChildren<RawImage>();

        if (data == null) return;
        if (cardNameText != null) cardNameText.text = data.cardName;
        if (artworkImage != null && data.artwork != null) artworkImage.texture = data.artwork;
        if (costText != null) costText.text = data.cost.ToString();
    }
}
