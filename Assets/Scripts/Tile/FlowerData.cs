using UnityEngine;
using ProtGardening;

[CreateAssetMenu(fileName = "FlowerData", menuName = "ScriptableObjects/FlowerData")]
public class FlowerData : ScriptableObject
{
    public string Name;
    public FlowerSprite Sprite;
    public ColorType Color;
    public int BuyPrice;
    public int SellPrice;
    public FlowerGrade Grade;

    [System.Serializable]
    public struct FlowerSprite
    {
        public Sprite Bloom;
        public Sprite Sprout;
        public Sprite Seed;
        public Sprite Shop;
    }
}
