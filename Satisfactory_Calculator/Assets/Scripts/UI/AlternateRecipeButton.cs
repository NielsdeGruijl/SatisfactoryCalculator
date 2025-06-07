using TMPro;
using UnityEngine;

public class AlternateRecipeButton : MonoBehaviour
{
    public AlternateRecipeManager alternateRecipeManager;
    public TMP_Text alternateRecipeName;
    public PartRecipe recipe;
        
    public void OnSelect()
    {
        alternateRecipeManager.ToggleAlternateRecipe(recipe);
    }
}
