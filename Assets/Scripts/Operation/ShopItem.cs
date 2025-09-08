using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProtGardening;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem", order = 1)]
public class ShopItem : ScriptableObject
{
    public Sprite ItemSprite;
    public string ItemName;
    public OperationType OperationType;
    public int Price;
    public FlowerData FlowerData;
    public int EffectAmount;
}
