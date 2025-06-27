using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    [SerializeField] private float targetOrthoSize = 10f;
    [SerializeField] private float defaultOrthoSize = 6f;
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private bool useZoomTarget = true;
    [SerializeField] private RectTransform zoomTarget;
    [SerializeField] private bool keepConfiner = false;
    [SerializeField] private CompositeCollider2D originalConfiner;
   

    private CinemachineCamera cinemachineCamera;
    private CinemachineConfiner2D cinemachineConfiner;
    private PlayerController player;
    private Coroutine zoomCoroutine;

    private void Awake()
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        if (cinemachineCamera == null)
            Debug.LogWarning("CinemachineCamera not found in the scene.");

        cinemachineConfiner = FindFirstObjectByType<CinemachineConfiner2D>();
        if (cinemachineConfiner == null)
            Debug.LogWarning("CinemachineConfiner2D not found in the scene.");

        player = FindFirstObjectByType<PlayerController>();
        if (player == null)
            Debug.LogWarning("PlayerController not found in the scene.");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || cinemachineCamera == null) return;

        StartSmoothZoom(targetOrthoSize);
        if (useZoomTarget) cinemachineCamera.Follow = zoomTarget != null ? zoomTarget : other.transform;
        if (cinemachineConfiner != null)
            if (keepConfiner) cinemachineConfiner.BoundingShape2D = originalConfiner;
            else cinemachineConfiner.BoundingShape2D = null;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player") || cinemachineCamera == null) return;

        StartSmoothZoom(defaultOrthoSize);
        cinemachineCamera.Follow = player != null ? player.transform : null;
        if (cinemachineConfiner != null && originalConfiner != null)
            cinemachineConfiner.BoundingShape2D = originalConfiner;
    }

    private void StartSmoothZoom(float size)
    {
        if (zoomCoroutine != null)
            StopCoroutine(zoomCoroutine);
        zoomCoroutine = StartCoroutine(SmoothZoom(size, zoomSpeed));
    }

    private IEnumerator SmoothZoom(float target, float speed)
    {
        while (Mathf.Abs(cinemachineCamera.Lens.OrthographicSize - target) > 0.01f)
        {
            cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(
                cinemachineCamera.Lens.OrthographicSize, target, Time.deltaTime * speed);
            yield return null;
        }
        cinemachineCamera.Lens.OrthographicSize = target;
        zoomCoroutine = null;
    }
}
