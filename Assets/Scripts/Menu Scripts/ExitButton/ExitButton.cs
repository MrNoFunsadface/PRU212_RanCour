using Assets.Scripts.Inventory;
using Scripts.Menu;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    [SerializeField]
    private OptionBar OptionBar;

    [SerializeField]
    private MenuPage InventoryUI;

    [SerializeField]
    private CharacterUI characterUI;

    void Start()
    {
        Hide();
    }

    public void OnExitButtonClick()
    {
        OptionBar.Hide();
        InventoryUI.Hide();
        characterUI.Hide();
        Hide();
    }

    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);
}
