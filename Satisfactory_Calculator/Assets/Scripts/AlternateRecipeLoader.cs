using System;
using UnityEditor;
using UnityEngine;

public class AlternateRecipeLoader : MonoBehaviour
{
    public PartRecipe[] recipes;
    
    private void Awake()
    {
        string[] paths = AssetDatabase.FindAssets("t:AlternateRecipeSO");
        recipes =  new PartRecipe[paths.Length];
        
        for (int i = 0; i < paths.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(paths[i]);
            var alternate = AssetDatabase.LoadAssetAtPath<AlternateRecipeSO>(path);
            recipes[i] = alternate.recipe;
        }
    }
}
