using System;
using System.Collections.Generic;
using UnityEngine;

public class AlternateRecipeManager : MonoBehaviour
{
    [Header("UI")] 
    [SerializeField] private GameObject alternateRecipeMenu;
    [SerializeField] private Transform availableRecipeContainer;
    [SerializeField] private Transform activeRecipeContainer;
    [SerializeField] private AlternateRecipeButton recipeButtonPrefab;
    
    public List<PartRecipe> alternateRecipes;
    
    private List<PartRecipe> activeRecipes;
    private List<PartRecipe> availableRecipes;

    private List<AlternateRecipeButton> generatedRecipeButtons;
    
    private PartsList partsList;
    private RecipeCalculator calculator;

    private bool isMenuOpen = false;
    
    private void Start()
    {
        partsList = GetComponent<PartsList>();
        calculator = GetComponent<RecipeCalculator>();
        
        alternateRecipeMenu.SetActive(false);
        
        generatedRecipeButtons = new List<AlternateRecipeButton>();
        
        foreach (Part part in partsList.parts)
        {
            part.activeRecipe = part.defaultRecipe;
        }
        
        activeRecipes = new List<PartRecipe>();
        availableRecipes = alternateRecipes;
        
        GenerateRecipeButtons();
    }

    private void GenerateRecipeButtons()
    {
        ClearRecipeButtons();
        
        foreach (PartRecipe partRecipe in activeRecipes)
        {
            AlternateRecipeButton recipeButton = Instantiate(recipeButtonPrefab, activeRecipeContainer);
            recipeButton.alternateRecipeManager = this;
            recipeButton.recipe = partRecipe;
            recipeButton.alternateRecipeName.text = partRecipe.name;
            generatedRecipeButtons.Add(recipeButton);
        }
        
        foreach (PartRecipe partRecipe in availableRecipes)
        {
            if (partRecipe.active)
                continue;
            AlternateRecipeButton recipeButton = Instantiate(recipeButtonPrefab, availableRecipeContainer);
            recipeButton.alternateRecipeManager = this;
            recipeButton.recipe = partRecipe;
            recipeButton.alternateRecipeName.text = partRecipe.name;
            generatedRecipeButtons.Add(recipeButton);
        }
    }

    private void ClearRecipeButtons()
    {
        int buttonCount = generatedRecipeButtons.Count;
        for(int i = 0; i < buttonCount; i++)
            Destroy(generatedRecipeButtons[i].gameObject);
        generatedRecipeButtons.Clear();
    }
    
    public void ToggleAlternateRecipe(PartRecipe recipe)
    {
        if (!recipe.active)
        {
            activeRecipes.Add(recipe);
            recipe.part.activeRecipe = recipe;
            recipe.active = true;
        }
        else
        {
            activeRecipes.Remove(recipe);
            if(recipe.part.activeRecipe == recipe)
                recipe.part.activeRecipe = recipe.part.defaultRecipe;
            recipe.active = false;
        }
        
        GenerateRecipeButtons();
        
        calculator.RegenerateRecipe();
    }

    public void OnOpenAlternateRecipeMenu()
    {
        alternateRecipeMenu.SetActive(true);
    }
    
    public void OnCloseAlternateRecipeMenu()
    {
        alternateRecipeMenu.SetActive(false);
    }
}
