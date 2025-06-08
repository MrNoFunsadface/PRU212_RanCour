using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Spotlight : MonoBehaviour
{
    public float minIntensity = 0.5f;
    public float maxIntensity = 1f;
    public float flickerSpeed = 0.1f;

    private Light2D light2D;
    private float timer;

    void Start()
    {
        light2D = GetComponent<Light2D>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            light2D.intensity = Random.Range(minIntensity, maxIntensity);
            timer = flickerSpeed;
        }
    }
}
