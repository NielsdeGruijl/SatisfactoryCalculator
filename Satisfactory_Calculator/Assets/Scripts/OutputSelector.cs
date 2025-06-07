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

    private void Start()
    {
        partsList = GetComponent<PartsList>();
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
