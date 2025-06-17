using System;
using UnityEngine;
[RequireComponent(typeof(Collider2D))]
public class Stairs : MonoBehaviour
{
    [Range(0, 180)]
    public float Angle;

    /// <summary>
    /// Gets the direction of the stairs vector
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDirection()
    {
        return Quaternion.AngleAxis(Angle, Vector3.forward) * Vector2.up;
    }

    /// <summary>
    /// Draws a line for us to easily tell which angle we are using for the horizontal stairs
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Vector2 direction = GetDirection();
        Vector2 origin = transform.position;
        Vector2 start = origin - direction.normalized * 0.5f;
        Vector2 end = origin + direction.normalized * 0.5f;
        Gizmos.DrawSphere(start, 0.03f);
        Gizmos.DrawSphere(end, 0.03f);
        Gizmos.DrawLine(start, end);
    }

    // Stairs entered
    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStairMovement player = other.gameObject.GetComponent<PlayerStairMovement>();
        if (player)
        {
            Debug.Log("Stairs entered by: " + other.name);
            player.CurrentStairs.Push(this);
        }
    }


    // Stairs exited
    void OnTriggerExit2D(Collider2D other)
    {
        PlayerStairMovement player = other.gameObject.GetComponent<PlayerStairMovement>();
        if (player)
        {
            player.CurrentStairs.Pop();
        }
    }

    // Debug log to confirm initialization
    private void Start()
    {
        Debug.Log("Stairs script initialized with angle: " + Angle);
    }
}
