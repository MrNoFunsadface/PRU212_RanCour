using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        Roaming,
        Despawned
    }

    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 initialPosition; // Store the starting position
    [SerializeField] private float roamRadius = 5f; // Adjust this to set the patrol area size

    private EnemyActiveState enemyActiveState;

    private void Awake()
    {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        enemyActiveState = GetComponent<EnemyActiveState>();
        initialPosition = transform.position; // Store the initial position
        state = State.Despawned;
    }

    public void SetToRoaming()
    {
        StopAllCoroutines();
        state = State.Roaming;
        enemyActiveState.Show();
        StartCoroutine(RoamingRoutine());
    }

    public void SetToDespawned()
    {
        StopAllCoroutines();
        state = State.Despawned;
        enemyActiveState.Hide();
    }

    private IEnumerator RoamingRoutine()
    {
        while (state == State.Roaming)
        {
            Vector2 roamPosition = GetRoamingPosition();
            enemyPathfinding.MoveTo(roamPosition);
            yield return new WaitForSeconds(2f);
        }
    }

    private Vector2 GetRoamingPosition()
    {
        // Generate a random direction and multiply by roamRadius
        Vector2 randomDirection = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        // Return a position within roamRadius of the initial position
        return initialPosition + randomDirection * Random.Range(0f, roamRadius);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(initialPosition, roamRadius);
    }
}