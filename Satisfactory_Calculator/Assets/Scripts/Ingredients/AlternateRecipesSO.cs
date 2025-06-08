using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AlternateRecipeList", menuName = "ScriptableObjects/AlternateRecipeList")]
public class AlternateRecipesSO : ScriptableObject
{
    public List<PartRecipe> alternateRecipes;
}
