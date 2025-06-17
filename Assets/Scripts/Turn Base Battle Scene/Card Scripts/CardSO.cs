using UnityEngine;

public enum CardCategory { Element, Compound, Catalyst }
public enum ElementalType { None, H2, O2, C, Na, Cl2, Fe, Al, Zn }
public enum DamageType { None, Thunder, Fire, Magic, Metal, Thermal, Blast }
public enum StatusEffect { None, Burn, Stun, Slow, Shield }

[CreateAssetMenu(fileName = "New Card", menuName = "Chemistry/Card", order = 1)]
public class CardSO : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    [TextArea] public string description;
    public Sprite artwork;
    public int cost;

    [Header("Card Type")]
    public CardCategory category;    // Element, Compound, Catalyst

    [Header("Chemical Properties")]
    public string formula;

    [Header("Element Effect (Elements & Compounds)")]
    public ElementalType elementType;
    public int elementDuration = 3;       // duration in rounds
    public bool isStackable = true;       // stack duration if re-applied
    public bool damageOnApplyOnly = true; // for compounds: damage only once on first apply

    [Header("Combat Stats")]
    public DamageType damageType;
    public int damage;
    public StatusEffect statusEffect;
    public int statusAmount;
    public bool ignoreArmor;
}
