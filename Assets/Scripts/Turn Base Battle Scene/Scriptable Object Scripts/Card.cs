using UnityEngine;

//
// Summary:
//     Card is a ScriptableObject that represents a card in the game.
//     It contains properties such as name, description, artwork, cost, and power.

[CreateAssetMenu(fileName = "New Card", menuName = "Turn Base/Card", order = 2)]
public class Card : ScriptableObject
{
    public string cardName;
    public string description;
    public Texture artwork;
    public int cost;
    public int power;
}