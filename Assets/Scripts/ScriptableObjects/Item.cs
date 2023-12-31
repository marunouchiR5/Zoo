using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game Data/Item/Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Texture2D Icon;
    public int Price;
}
