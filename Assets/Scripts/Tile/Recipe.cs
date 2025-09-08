using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe")]
public class Recipe : ScriptableObject
{
    public FlowerData ParentA;
    public FlowerData ParentB;
    public FlowerData Result;
}