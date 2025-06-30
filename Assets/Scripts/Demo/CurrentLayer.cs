using UnityEngine;

public class CurrentLayer : MonoBehaviour
{
    private int layer = 2;

    public void SetLayer(int newLayer)
    {
        layer = newLayer;
    }

    public int GetLayer()
    {
        return layer;
    }
}
