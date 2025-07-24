using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menu
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [SerializeField]
        private Image itemSprite;

        [SerializeField]
        private TMP_Text atkStats;

        [SerializeField]
        private TMP_Text defStats;

        [SerializeField]
        private TMP_Text itemName;

        [SerializeField]
        private TMP_Text itemDescription;

        [SerializeField]
        private TMP_Text quirk;

        [SerializeField]
        private TMP_Text quirkDescription;

        private void Awake()
        {
            ResetDescription();
            itemSprite.preserveAspect = true;
            itemSprite.type = Image.Type.Simple;
        }

        public void ResetDescription()
        {
            itemSprite.gameObject.SetActive(false);
            atkStats.text = "";
            defStats.text = "";
            itemName.text = "";
            itemDescription.text = "";
            quirk.text = "";
            quirkDescription.text = "";
        }

        public void SetDescription(Sprite sprite, int atkStats, int defStats, string itemName, string itemDescription, string quirk, string quirkDescription)
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = sprite;

            SetDescriptionVisibility(atkStats, defStats, itemName, itemDescription, quirk, quirkDescription);

            this.atkStats.text = "atk: " + atkStats.ToString();
            this.defStats.text = "def: " + defStats.ToString();
            this.itemName.text = itemName;
            this.itemDescription.text = itemDescription;
            this.quirk.text = quirk;
            this.quirkDescription.text = quirkDescription;

            AdjustFontSizes(quirk, quirkDescription);
        }

        private void AdjustFontSizes(string quirk, string quirkDescription)
        {
            this.itemName.fontSize = this.itemName.text.Length < 10 ? 40 : (this.itemName.text.Length > 10 ? 25 : this.itemName.text.Length); // Adjust font size based on length
            this.itemDescription.fontSize = this.itemDescription.text.Length > 50 ? 14 : 16; // Adjust font size based on length
            this.quirk.fontSize = quirk.Length > 20 ? 20 : quirk.Length; // Adjust font size based on length
            this.quirkDescription.fontSize = quirkDescription.Length > 50 ? 14 : 16; // Adjust font size based on length
        }

        private void SetDescriptionVisibility(int atkStats, int defStats, string itemName, string itemDescription, string quirk, string quirkDescription)
        {
            if (atkStats < 1) this.atkStats.gameObject.SetActive(false);
            else this.atkStats.gameObject.SetActive(true);
            if (defStats < 1) this.defStats.gameObject.SetActive(false);
            else this.defStats.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(itemName)) this.itemName.gameObject.SetActive(false);
            else this.itemName.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(itemDescription)) this.itemDescription.gameObject.SetActive(false);
            else this.itemDescription.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(quirk)) this.quirk.gameObject.SetActive(false);
            else this.quirk.gameObject.SetActive(true);
            if (string.IsNullOrEmpty(quirkDescription)) this.quirkDescription.gameObject.SetActive(false);
            else this.quirkDescription.gameObject.SetActive(true);
        }
    }
}