using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RecipeBook", menuName = "ScriptableObjects/RecipeBook")]
public class RecipeBook : ScriptableObject
{
    public List<Recipe> Recipes;
    private Dictionary<(FlowerData, FlowerData), FlowerData> recipeLookup;

    private void OnEnable()
    {
        recipeLookup = new Dictionary<(FlowerData, FlowerData), FlowerData>();

        foreach (var recipe in Recipes)
        {
            recipeLookup[(recipe.ParentA, recipe.ParentB)] = recipe.Result;
        }
    }

    public bool TryHybridize(Flower flowerA, Flower flowerB, out FlowerData result)
    {
        return recipeLookup.TryGetValue((flowerA.GetFlowerData(), flowerB.GetFlowerData()), out result);
    }
}
