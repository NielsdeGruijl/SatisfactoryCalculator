using System.Collections.Generic;
using UnityEngine;

public class OutputSelector : MonoBehaviour
{
    [SerializeField] private RecipeCalculator calculator;
    
    [Header("OutputMenu UI")]
    [SerializeField] private Transform outputSelectorMenu;
    [SerializeField] private Transform outputMenuItemContainer;
    [SerializeField] private OutputMenuItem outputMenuItemPrefab;

    private PartsList partsList;

    private List<Part> sortedPartsList;
    
    private void Start()
    {
        partsList = GetComponent<PartsList>();

        sortedPartsList = new List<Part>(partsList.parts);
        
        outputSelectorMenu.gameObject.SetActive(false);
        GenerateOutputMenu();
    }

    private void GenerateOutputMenu()
    {
        foreach (Part part in partsList.parts)
        {
            OutputMenuItem outputMenuItem = Instantiate(outputMenuItemPrefab, outputMenuItemContainer);
            outputMenuItem.SetPart(part);
            outputMenuItem.outputSelector = this;
        }
    }

    private void SortParts()
    {
        
    }
    
    public void SelectOutputPart(Part part)
    {
        calculator.SetOutputPart(part);
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
