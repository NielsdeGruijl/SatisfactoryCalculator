using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Part", menuName = "ScriptableObjects/Part")]
public class Part : ScriptableObject
{
    public Sprite icon;

    public PartRecipe defaultRecipe;
    public PartRecipe activeRecipe;
    
    public Producer producer;
}