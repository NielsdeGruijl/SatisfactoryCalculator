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

    public PartRecipe(PartRecipe other)
    {
        if (other == null)
            return;
        
        name = other.name;
        part = other.part;
        
        productionIcon = other.productionIcon;
        productionPerMinute = other.productionPerMinute;
        
        active = other.active;
        partIngredients = new List<PartIngredient>(other.partIngredients);
        materialIngredients = new List<MaterialIngredient>(other.materialIngredients);
    }
}
