using UnityEngine;

//
// Summary:
//     Card is a ScriptableObject that represents a card in the game.
//     It contains properties such as name, description, artwork, cost, and power.

public enum CardCategory { Element, Compound, Catalyst }
public enum ElementalType
{
    None,

    // Elements
    H2, O2, C, Na, Cl2, Fe, Al, Zn,

    // Simple Compounds
    HCl,   // hydrochloric acid
    NaOH,  // sodium hydroxide
    NaCl,  // salt
    H2O,   // water
    CO2,   // carbon dioxide
    AgCl,  // silver chloride
    ZnSO4, // zinc sulfate
    Cu,    // copper (product)
    CuSO4, // optional nomenclature if you want separate
    Fe2O3, // iron(III) oxide
    Al2O3, // aluminum oxide
    AgNO3, // silver nitrate
    NaNO3, // sodium nitrate
    CH4,
    // …add more as needed
}

public enum DamageType { None, Thunder, Fire, Magic, Metal, Thermal, Blast }
public enum StatusEffect { None, Burn, Stun, Slow, Shield }

[CreateAssetMenu(fileName = "New Card", menuName = "Turn Base/Card/Card", order = 2)]
public class CardSO : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;
    [TextArea] public string description;
    public Texture artwork;
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