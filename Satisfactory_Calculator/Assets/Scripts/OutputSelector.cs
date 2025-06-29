using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class OutputSelector : MonoBehaviour
{
    [SerializeField] private RecipeCalculator calculator;
    [SerializeField] private WorldRecipeCalculator worldCalculator;
    
    [Header("OutputMenu UI")]
    [SerializeField] private Transform outputSelectorMenu;
    [SerializeField] private Transform outputMenuItemContainer;
    [SerializeField] private OutputMenuItem outputMenuItemPrefab;

    private PartLoader partLoader;
    
    private void Start()
    {
        partLoader = GetComponent<PartLoader>();
        outputSelectorMenu.gameObject.SetActive(false);
        GenerateOutputMenu();
    }

    private void GenerateOutputMenu()
    {
        foreach (Part part in partLoader.parts)
        {
            OutputMenuItem outputMenuItem = Instantiate(outputMenuItemPrefab, outputMenuItemContainer);
            outputMenuItem.SetPart(part);
            outputMenuItem.outputSelector = this;
        }
    }

    public void SelectOutputPart(Part part)
    {
        if(calculator)
            calculator.SetOutputPart(part);
        if(worldCalculator)
            worldCalculator.OnSetOutputPart(part);
        
        outputSelectorMenu.gameObject.SetActive(false);
    }

    public void OnOpenOutputMenu()
    {
        outputSelectorMenu.gameObject.SetActive(true);
    }

    public void OnCloseOutputMenu()
    {
        outputSelectorMenu.gameObject.SetActive(false);
    }
}
