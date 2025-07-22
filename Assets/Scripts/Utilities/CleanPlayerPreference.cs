using UnityEngine;

public class CleanPlayerPreference : MonoBehaviour
{
    public void CleanPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("Player preferences cleaned successfully.");
    }
}
