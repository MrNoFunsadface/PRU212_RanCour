using System.Collections;
using System.Collections.Generic;
using System.Linq;  // for OrderBy, Concat
using UnityEngine;
using static CombatSystem;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance;

    [Header("Who takes turns")]
    public PlayerControllerCombat player;
    public List<EnemyCombatAI> enemies;

    private Queue<ITurnTaker> queue = new Queue<ITurnTaker>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        queue.Clear();
        queue.Enqueue(player);
        foreach (var e in enemies)
            queue.Enqueue(e);

        StartCoroutine(RunTurns());
    }

    private IEnumerator RunTurns()
    {
        while (player.IsAlive && enemies.Any(e => e.IsAlive))
        {
            var actor = queue.Dequeue();
            if (actor.IsAlive)
                yield return actor.TakeTurn();
            queue.Enqueue(actor);
        }
        Debug.Log("Battle Over!");
    }
}
