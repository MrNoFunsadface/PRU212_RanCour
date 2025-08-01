using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Reaction", menuName = "Turn Base/Reaction", order = 2)]

public class ReactionSO : ScriptableObject
{
    public string reactionName;
    [TextArea] public string equation;

    [Header("Input Reactants (exact counts)")]
    public List<ElementalType> inputElements;

    [Header("Combat Effect")]
    public DamageType damageType;
    public int damage;
    public StatusEffect statusEffect;
    public int statusAmount;
    public bool ignoreArmor;
}
