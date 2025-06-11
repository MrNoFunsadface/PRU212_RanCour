using UnityEngine;

//
// Summary:
//     DropZoneScript is responsible for managing the drop zones in the battle scene.
//     It holds information about the enemy's name and order, which can be used to identify
//     and manage enemies during gameplay.
public class DropZoneScript : MonoBehaviour
{
    public string enemyName;
    public int enemyOrder;
    public GameObject healthBar;
    public GameObject activeHealthBar;
}
