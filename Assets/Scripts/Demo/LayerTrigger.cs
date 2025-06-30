using Unity.Cinemachine;
using UnityEngine;

public class LayerTrigger : MonoBehaviour
{
    [SerializeField]
    private Collider2D layerTrigger;

    [SerializeField]
    private CompositeCollider2D CameraBoundOn;

    [SerializeField]
    private CompositeCollider2D[] CameraBoundsOff;

    [SerializeField]
    private CinemachineConfiner2D cinemachine;

    [SerializeField]
    private GameObject bridgePart;

    [SerializeField]
    private CurrentLayer currentLayer;

    [SerializeField]
    private int layer = 2;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var cameraBound in CameraBoundsOff)
        {
            if (cameraBound != null)
            {
                cameraBound.gameObject.SetActive(false);
            }
        }

        if (CameraBoundOn != null)
        {
            CameraBoundOn.gameObject.SetActive(true);
        }
        //cinemachine.BoundingShape2D = CameraBoundOn;

        currentLayer.SetLayer(layer);
        if (currentLayer.GetLayer() == 1) bridgePart.SetActive(true);
        else bridgePart.SetActive(false);
    }
}
