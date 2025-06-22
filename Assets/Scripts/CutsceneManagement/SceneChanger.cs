using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    public float changeTime;
    public string sceneName;

    // Update is called once per frame
    void Update()
    {
        changeTime -= Time.deltaTime;
        if (changeTime <= 0f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
