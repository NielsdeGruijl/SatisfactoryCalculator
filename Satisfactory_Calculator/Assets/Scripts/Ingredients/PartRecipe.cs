using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PartRecipe
{
    [Header(("Part"))]
    public string name;
    public Part part;
    
    [Header("Production")]
    public Sprite productionIcon;
    public float productionPerMinute;
    
    [Header("Recipe")]
    public bool active;
    public List<PartIngredient> partIngredients;
    public List<MaterialIngredient> materialIngredients;
}
