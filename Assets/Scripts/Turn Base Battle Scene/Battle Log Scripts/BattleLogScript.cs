using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BattleLogScript : MonoBehaviour
{
    [SerializeField] private List<string> battleLogText = new List<string>();
    [SerializeField] private TextMeshProUGUI displayer;

    private void Update()
    {
    }

    public void LogBattleEvent(string message)
    {
        if (string.IsNullOrWhiteSpace(message))
            return;
        battleLogText.Add(message);
    }

    public void UpdateDisplayer()
    {
        displayer.text = string.Empty; // Clear the displayer text
        foreach (string text in battleLogText)
        {
            if (!string.IsNullOrEmpty(text))
            {
                displayer.text += text + "\n"; // Append each log message
            }
        }
    }
}
