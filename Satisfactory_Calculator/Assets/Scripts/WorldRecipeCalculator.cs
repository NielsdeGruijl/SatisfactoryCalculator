using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorldRecipeCalculator : MonoBehaviour
{
    [SerializeField] private Part outputPart;
    [SerializeField] private Transform container;
    
    [Header("Production Tree")]
    [SerializeField] private Transform productionTree;
    [SerializeField] private ProductionNode baseProductionNode;
    [SerializeField] private ProductionNode productionNodePrefab;
    [SerializeField] private MaterialNode materialNodePrefab;
    [SerializeField] private LineDrawer lineDrawerPrefab;

    [Header("Input List")] 
    [SerializeField] private Transform inputContainer;
    [SerializeField] private CostItem inputItemPrefab;

    [Header("Output Info")]
    [SerializeField] private TMP_InputField amountInputField;
    [SerializeField] private Image outputImage;
    [SerializeField] private TMP_Text outputName;
    
    [Header("Tools")] 
    [SerializeField] private Transform verticalContainer;
    [SerializeField] private Transform horizontalContainer;

    private List<GameObject> lines = new List<GameObject>();
    private List<GameObject> productionBranches = new List<GameObject>();
    private List<GameObject> branchBaseNodes = new List<GameObject>();
    private List<GameObject> inputs = new List<GameObject>();

    private Dictionary<Part, float> inputParts;
    private Dictionary<Material, float> inputMaterials;

    private float amountToCraft = 1;
    
    private void Start()
    {
        SetOutputPart(outputPart);
        GenerateTree();
    }

    private void SetOutputPart(Part part)
    {
        outputPart = part;
        amountInputField.text = "1";
        outputImage.sprite = outputPart.icon;
        outputName.text = outputPart.name;
    }
    
    public void GenerateTree()
    {
        ClearProductionTree();
        
        inputParts = new Dictionary<Part, float>();
        inputMaterials = new Dictionary<Material, float>();
        
        CreateProductionNode(baseProductionNode, outputPart, amountToCraft);
        
        foreach (PartIngredient ingredient in outputPart.activeRecipe.partIngredients)
        {
            GenerateBranch(ingredient);
        }

        foreach (MaterialIngredient ingredient in outputPart.activeRecipe.materialIngredients)
        {
            if(!inputMaterials.TryAdd(ingredient.material, ingredient.amount * amountToCraft))
                inputMaterials[ingredient.material] += ingredient.amount * amountToCraft;
        }

        for (int i = inputMaterials.Count; i > 0; i--)
        {
            int index = i - 1;
            CostItem inputMaterialObject = Instantiate(inputItemPrefab, inputContainer);
            inputMaterialObject.icon.sprite = inputMaterials.ElementAt(index).Key.icon;
            inputMaterialObject.amountText.text = inputMaterials.ElementAt(index).Value.ToString("0.0");
            inputs.Add(inputMaterialObject.gameObject);
        }
        
        for (int i = inputParts.Count; i > 0; i--)
        {
            int index = i - 1;
            CostItem inputPartObject = Instantiate(inputItemPrefab, inputContainer);
            inputPartObject.icon.sprite =  inputParts.ElementAt(index).Key.icon;
            inputPartObject.amountText.text = inputParts.ElementAt(index).Value.ToString("0.0");
            inputs.Add(inputPartObject.gameObject);
        }
        
        /*
        foreach (KeyValuePair<Part, float> pair in inputParts)
        {

        }
        
        foreach (KeyValuePair<Material, float> pair in inputMaterials)
        {

        }*/
    }
    
    private void GenerateBranch(PartIngredient baseIngredient)
    {
        List<Dictionary<ProductionNode, float>> tiers = new List<Dictionary<ProductionNode, float>>();
        
        Transform branchContainer = Instantiate(horizontalContainer, productionTree);

        productionBranches.Add(branchContainer.gameObject);
        
        // Branch base node
        ProductionNode branchBaseNode = Instantiate(productionNodePrefab, branchContainer);
        branchBaseNode.parent = branchContainer;
        CreateProductionNode(branchBaseNode, baseIngredient.part, baseIngredient.amount * amountToCraft);
        
        if(!inputParts.TryAdd(baseIngredient.part, baseIngredient.amount * amountToCraft))
            inputParts[baseIngredient.part] += baseIngredient.amount * amountToCraft;
        
        branchBaseNodes.Add(branchBaseNode.gameObject);
        
        CreateLineDrawer(branchBaseNode.GetComponent<RectTransform>(), baseProductionNode.GetComponent<RectTransform>());

        Dictionary<ProductionNode, float> baseTier = new Dictionary<ProductionNode, float>();
        baseTier.Add(branchBaseNode, amountToCraft);
        tiers.Add(baseTier);
        
        while (tiers.Count > 0)
        {
            Dictionary<ProductionNode, float> nextTier = new Dictionary<ProductionNode, float>();
            
            foreach (KeyValuePair<ProductionNode, float> pair in tiers[0])
            {
                ProductionNode currentProductionNode = pair.Key;
                Part currentPart = currentProductionNode.part;
                float currentAmountToCraft = pair.Value;

                Transform tierContainer = null;
                
                foreach (PartIngredient ingredient in currentPart.activeRecipe.partIngredients)
                {
                    ProductionNode newProductionNode;
                    
                    if(!inputParts.TryAdd(ingredient.part, ingredient.amount * currentAmountToCraft))
                        inputParts[ingredient.part] += ingredient.amount * currentAmountToCraft;
                    
                    Transform nodeContainer = null;
                    if (currentPart.activeRecipe.partIngredients.Count + currentPart.activeRecipe.materialIngredients.Count > 1)
                    {
                        if (tierContainer == null)
                        {
                            tierContainer = Instantiate(verticalContainer, currentProductionNode.parent);
                        }
                        
                        nodeContainer = Instantiate(horizontalContainer, tierContainer);
                        
                        newProductionNode = Instantiate(productionNodePrefab, nodeContainer);
                        newProductionNode.parent = nodeContainer;
                    }
                    else
                    {
                        newProductionNode = Instantiate(productionNodePrefab, currentProductionNode.parent);
                        newProductionNode.parent = currentProductionNode.parent;
                    }
                    
                    CreateProductionNode(newProductionNode, ingredient.part, ingredient.amount * currentAmountToCraft);
                    
                    CreateLineDrawer(newProductionNode.GetComponent<RectTransform>(), currentProductionNode.GetComponent<RectTransform>());
                    
                    nextTier.Add(newProductionNode, ingredient.amount * currentAmountToCraft);
                }
                
                foreach (MaterialIngredient ingredient in currentPart.activeRecipe.materialIngredients)
                {
                    if(!inputMaterials.TryAdd(ingredient.material, ingredient.amount * currentAmountToCraft))
                        inputMaterials[ingredient.material] += ingredient.amount * currentAmountToCraft;

                    MaterialNode node;
                    Transform nodeContainer = null;
                    if (tierContainer != null)
                    {
                        nodeContainer = Instantiate(horizontalContainer, tierContainer);
                        node = Instantiate(materialNodePrefab, nodeContainer);
                    }
                    else
                    {
                        if (currentPart.activeRecipe.partIngredients.Count + currentPart.activeRecipe.materialIngredients.Count > 1)
                        {
                            tierContainer = Instantiate(verticalContainer, currentProductionNode.parent);
                            nodeContainer = Instantiate(horizontalContainer, tierContainer);
                            
                            node = Instantiate(materialNodePrefab, nodeContainer);
                        }
                        else
                        {
                            node = Instantiate(materialNodePrefab, currentProductionNode.parent);
                        }
                    }
                    
                    /*if (currentPart.activeRecipe.partIngredients.Count + currentPart.activeRecipe.materialIngredients.Count > 1)
                    {
                        if (tierContainer == null)
                        {
                            tierContainer = Instantiate(verticalContainer, currentProductionNode.parent);
                            nodeContainer = Instantiate(horizontalContainer, tierContainer);
                        }
                        else if (tierContainer != null)
                        {
                            nodeContainer = Instantiate(horizontalContainer, tierContainer);
                        }
                        else
                        {
                            nodeContainer = Instantiate(verticalContainer, currentProductionNode.parent);
                        }
                        
                        node = Instantiate(materialNodePrefab, nodeContainer);
                    }
                    else
                    {
                    }*/
                    
                    node.material = ingredient.material;
                    node.amount.text = (ingredient.amount * currentAmountToCraft).ToString("0.0");
                    node.icon.sprite = ingredient.material.icon;
                    
                    CreateLineDrawer(node.GetComponent<RectTransform>(), currentProductionNode.GetComponent<RectTransform>());
                }
            }
            
            if(nextTier.Count > 0)
                tiers.Add(nextTier);
            
            tiers.RemoveAt(0);
        }
    }

    private void ClearProductionTree()
    {
        int lineCount =  lines.Count;
        for (int i = 0; i < lineCount; i++)
            Destroy(lines[i]);
        lines = new List<GameObject>();
        
        int productionBranchesCount = productionBranches.Count;
        for (int i = 0; i < productionBranchesCount; i++)
            Destroy(productionBranches[i]);
        productionBranches = new List<GameObject>();
        
        int branchBaseCount = branchBaseNodes.Count;
        for (int i = 0; i < branchBaseCount; i++)
            Destroy(branchBaseNodes[i]);
        branchBaseNodes = new List<GameObject>();
        
        int inputCount = inputs.Count;
        for(int i = 0; i < inputCount; i++)
            Destroy(inputs[i].gameObject);
        inputs = new List<GameObject>();
    }

    private void CreateProductionNode(ProductionNode productionNode, Part part, float amount)
    {
        productionNode.part = part;
        productionNode.icon.sprite = part.activeRecipe.productionIcon;
        productionNode.partIcon.sprite = part.icon;
        productionNode.productionPerMinute.text = part.activeRecipe.productionPerMinute + " p/m";
        productionNode.quantity.text = (amount / part.activeRecipe.productionPerMinute).ToString("0.00") + "x";
    }    
    
    private void CreateLineDrawer(RectTransform from, RectTransform to)
    {
        LineDrawer line = Instantiate(lineDrawerPrefab, from);
        line.width = 0.2f;
        line.DrawLine(from, to);
        
        lines.Add(line.gameObject);
    }

    public void OnSetOutputPart(Part part)
    {
        SetOutputPart(part);
        GenerateTree();
    }

    public void OnChangeAmountToCraft()
    {
        if (amountInputField.text == "")
        {
            amountInputField.text = "1";
            amountToCraft = 1;
        }
        else
        {
            amountToCraft = float.Parse(amountInputField.text);
        }

        GenerateTree();
    }
}