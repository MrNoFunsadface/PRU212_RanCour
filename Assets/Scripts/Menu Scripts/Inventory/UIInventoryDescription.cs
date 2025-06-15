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

        public void SetDescription(Sprite sprite, int atkStats, int defStats, string itemName, string itemDescription, string quirk, string quirktDescription)
        {
            itemSprite.gameObject.SetActive(true);
            itemSprite.sprite = sprite;
            this.atkStats.text = "atk: " + atkStats.ToString();
            this.defStats.text = "def: " + defStats.ToString();
            this.itemName.text = itemName;
            this.itemDescription.text = itemDescription;
            this.quirk.text = quirk;
            quirkDescription.text = quirktDescription;
        }
    }
}