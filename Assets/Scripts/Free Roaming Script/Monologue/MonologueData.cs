using UnityEngine;

[CreateAssetMenu(fileName = "MonologueData", menuName = "Scriptable Objects/MonologueData")]
public class MonologueData : ScriptableObject
{
    [TextArea(2, 5)]
    public string monologueText;

    public string speakerName;
}
