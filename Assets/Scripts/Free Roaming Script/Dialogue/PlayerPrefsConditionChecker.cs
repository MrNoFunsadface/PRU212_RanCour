using UnityEngine;

public class PlayerPrefsConditionChecker : IConditionChecker
{
    public bool IsConditionMet(string key)
    {
        return PlayerPrefs.GetInt(key, 0) == 1;
    }
}
