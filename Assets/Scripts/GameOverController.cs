// GameOverController.cs
using UnityEngine;
using UnityEngine.UI;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private GameObject winPanel;   // drag in your Win UI
    [SerializeField] private GameObject losePanel;  // drag in your Lose UI

    public void ShowWin()
    {
        winPanel.SetActive(true);
        // TODO: disable other inputs
    }

    public void ShowLose()
    {
        losePanel.SetActive(true);
        // TODO: disable other inputs
    }
}
