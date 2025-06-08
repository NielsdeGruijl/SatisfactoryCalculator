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
    
    public AlternateRecipesSO alternateRecipesSO;

    public PartListSO partsListSO;
    
    public List<PartRecipe> alternateRecipes;
    private List<PartRecipe> activeRecipes;
    private List<PartRecipe> availableRecipes;

    private List<AlternateRecipeButton> generatedRecipeButtons;
    
    private RecipeCalculator calculator;
    private WorldRecipeCalculator worldCalculator;

    private void Start()
    {
        calculator = GetComponent<RecipeCalculator>();
        worldCalculator = GetComponent<WorldRecipeCalculator>();

        //alternateRecipesSO.alternateRecipes = new List<PartRecipe>(alternateRecipes);
        
        alternateRecipeMenu.SetActive(false);
        
        generatedRecipeButtons = new List<AlternateRecipeButton>();
        
        foreach (Part part in partsListSO.parts)
        {
            part.activeRecipe = part.defaultRecipe;
        }
        
        activeRecipes = new List<PartRecipe>();
        availableRecipes = new List<PartRecipe>(alternateRecipesSO.alternateRecipes);
        
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
        
        //calculator.RegenerateRecipe();
        worldCalculator.GenerateTree();
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
