using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "ScriptableObjects/Part")]
public class Part : ScriptableObject
{
    public Sprite icon;

    public int tier;
    
    public PartRecipe defaultRecipe;
    public PartRecipe activeRecipe;
    
}