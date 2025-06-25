using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraZoomTrigger : MonoBehaviour
{
    [SerializeField]
    private float targetOrthoSize = 10f;

    private float defaultOrthoSize = 6f;

    [SerializeField]
    private float zoomSpeed = 10f;

    [SerializeField]
    private CinemachineCamera cinemachineCamera;

    private Coroutine zoomCoroutine;

    [SerializeField]
    private RectTransform zoomTarget;

    [SerializeField]
    private CapsuleCollider2D player;

    //Confinder2D settings
    [SerializeField]
    private CinemachineConfiner2D cinemachineConfinder;

    [SerializeField]
    private CompositeCollider2D originalConfinder;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var vcam = cinemachineCamera != null ? cinemachineCamera : FindFirstObjectByType<CinemachineCamera>();
            if (vcam != null)
            {
                SmoothZoom(vcam, targetOrthoSize);
                vcam.Follow = zoomTarget != null ? zoomTarget : other.transform;
                cinemachineConfinder.BoundingShape2D = player;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var vcam = cinemachineCamera != null ? cinemachineCamera : FindFirstObjectByType<CinemachineCamera>();
            if (vcam != null)
            {
                SmoothZoom(vcam, defaultOrthoSize);
                vcam.Follow = player != null ? player.transform : null;
                if (originalConfinder != null)
                {
                    cinemachineConfinder.BoundingShape2D = originalConfinder;
                }
                else
                {
                    cinemachineConfinder.BoundingShape2D = null;
                }
            }
        }
    }

    private void NormalZoom(CinemachineCamera vcam, float size)
    {
        vcam.Lens.OrthographicSize = size;
    }

    private void SmoothZoom(CinemachineCamera vcam, float size)
    {
        if (zoomCoroutine != null)
            vcam.StopCoroutine(zoomCoroutine);
        zoomCoroutine = vcam.StartCoroutine(SmoothZoom(vcam, size, zoomSpeed));
    }

    private IEnumerator SmoothZoom(CinemachineCamera vcam, float target, float speed)
    {
        while (Mathf.Abs(vcam.Lens.OrthographicSize - target) > 0.01f)
        {
            vcam.Lens.OrthographicSize = Mathf.Lerp(
                vcam.Lens.OrthographicSize, target, Time.deltaTime * speed);
            yield return null;
        }
        vcam.Lens.OrthographicSize = target;
        zoomCoroutine = null;
    }
}
