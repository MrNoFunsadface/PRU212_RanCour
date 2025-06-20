using UnityEngine;

public class EnemyActiveState : MonoBehaviour
{
    public void Hide() => gameObject.SetActive(false);

    public void Show() => gameObject.SetActive(true);

}
