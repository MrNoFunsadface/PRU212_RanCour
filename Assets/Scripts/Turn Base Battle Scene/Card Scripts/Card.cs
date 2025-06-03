using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Card", order = 2)]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Sprite artwork;
    public int cost;
    public int power;
}