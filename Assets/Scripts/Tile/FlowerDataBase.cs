using UnityEngine;
using System.Collections.Generic;
using ProtGardening;

[CreateAssetMenu(fileName = "FlowerDataBase", menuName = "ScriptableObjects/FlowerDataBase")]
public class FlowerDataBase : ScriptableObject
{
    public List<FlowerData> Flowers;

    public FlowerData GetFlowerDataByName(string name)
    {
        return Flowers.Find(flower => flower.Name == name);
    }
}