using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeCalculator : MonoBehaviour
{
    public Part outputPart;

    [Header("Output")]
    [SerializeField] private Image outputImage;
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private TMP_InputField amountText;

    [Header("CostList")] 
    [SerializeField] private Transform costContainer;
    [SerializeField] private CostItem costPrefab;
    
    [Header("ProductionTree")]
    [SerializeField] private Transform productionTree;
    [SerializeField] private Transform productionBranchPrefab;
    [SerializeField] private Transform productionNodeContainerPrefab;
    [SerializeField] private Transform branchBaseContainer;
    [SerializeField] private ProductionNode startingProductionNode;
    [SerializeField] private ProductionNode productionNodePrefab;
    [SerializeField] private LineDrawer lineDrawerPrefab;
    
    RectTransform rectTransform;
    
    private List<Transform> recipeTiersUI;
    private List<CostItem> costs;

    private List<GameObject> lines;
    private List<ProductionNode> productionBranchBases;
    
    private Dictionary<Part, float> costParts;
    Dictionary<Material, float> costMaterials;

    private float amountToCraft = 1;

    private bool can;

    private void Start()
    {
        recipeTiersUI = new List<Transform>();
        lines = new List<GameObject>();
        costs = new List<CostItem>();
        productionBranchBases = new List<ProductionNode>();
        
        amountText.text = "1";
        
        GenerateRecipe(outputPart);
    }

    public void SetOutputPart(Part part)
    {
        outputPart = part;
        GenerateRecipe(outputPart);
    }
    
    private void GenerateRecipe(Part pOutputPart)
    {
        ClearRecipe();
        outputImage.sprite = outputPart.icon;
        outputText.text = pOutputPart.name;
        can = true;
        CreateProductionTree();
    }

    public void RegenerateRecipe()
    {
        ClearRecipe();
        CreateProductionTree();
    }
    
    private void CreateProductionTree()
    {
        costParts = new Dictionary<Part, float>();
        costMaterials = new Dictionary<Material, float>();

        SetUpProductionNode(startingProductionNode, outputPart, amountToCraft);

        PartRecipe recipe = outputPart.activeRecipe;
        
        foreach (PartIngredient partIngredient in recipe.partIngredients)
        {
            CreateProductionBranch(partIngredient.part, partIngredient.amount * amountToCraft);
        }

        foreach (MaterialIngredient ingredient in recipe.materialIngredients)
        {
            if (!costMaterials.TryAdd(ingredient.material, ingredient.amount * amountToCraft))
                costMaterials[ingredient.material] += ingredient.amount * amountToCraft;
        }
        
        for (int i = costMaterials.Count; i > 0; i--)
        {
            CostItem itemObject = Instantiate(costPrefab, costContainer);
            itemObject.icon.sprite = costMaterials.ElementAt(i - 1).Key.icon;
            itemObject.amountText.text = costMaterials.ElementAt(i - 1).Value.ToString("0.0");
            costs.Add(itemObject);
        }
        
        for (int i = costParts.Count; i > 0; i--)
        {
            CostItem itemObject = Instantiate(costPrefab, costContainer);
            itemObject.icon.sprite = costParts.ElementAt(i - 1).Key.icon;
            itemObject.amountText.text = costParts.ElementAt(i - 1).Value.ToString("0.0");
            costs.Add(itemObject);
        }
    }

    private void CreateProductionBranch(Part pPart, float pAmount)
    {
        List<Dictionary<Part, float>> tiers = new List<Dictionary<Part, float>>();
        Dictionary<Part, float> outputTier = new Dictionary<Part, float>();
        outputTier.Add(pPart, pAmount);
        tiers.Add(outputTier);

        PartRecipe baseRecipe = pPart.activeRecipe;
        
        if (!costParts.TryAdd(pPart, pAmount))
            costParts[pPart] += pAmount;
        
        Transform productionBranchObject =  Instantiate(productionBranchPrefab, productionTree);
        recipeTiersUI.Add(productionBranchObject);
        
        //Transform tproductionNodeContainerObject =  Instantiate(productionNodeContainerPrefab, productionBranchObject);
        
        ProductionNode tProductionNodeObject = Instantiate(productionNodePrefab, branchBaseContainer);
        SetUpProductionNode(tProductionNodeObject, pPart, pAmount);
        productionBranchBases.Add(tProductionNodeObject);
        
        Canvas.ForceUpdateCanvases();
        CreateLineDrawer(startingProductionNode.GetComponent<RectTransform>(), tProductionNodeObject.GetComponent<RectTransform>());

        // set line drawer targets for ingredients of production branch base part
        foreach (PartIngredient partIngredient in baseRecipe.partIngredients)
        {
            partIngredient.productionNode = tProductionNodeObject;
        }
        
        while (tiers.Count > 0)
        {
            Dictionary<Part, float> thisTier = tiers.ElementAt(0);
            Dictionary<Part, float> nextTier = new Dictionary<Part, float>();

            bool canMakeNodeContainer = true;

            Transform productionNodeContainerObject = null;
            
            for (int i = 0; i < thisTier.Count; i++)
            {
                Part part = thisTier.ElementAt(i).Key;
                float amountToCraft = thisTier[part];

                PartRecipe recipe = part.activeRecipe;

                foreach (PartIngredient ingredient in recipe.partIngredients)
                {
                    if (canMakeNodeContainer)
                    {
                        productionNodeContainerObject = Instantiate(productionNodeContainerPrefab, productionBranchObject);
                        canMakeNodeContainer = false;
                    }
                    
                    if (!costParts.TryAdd(ingredient.part, ingredient.amount * amountToCraft))
                        costParts[ingredient.part] += ingredient.amount * amountToCraft;
                    
                    ProductionNode productionNodeObject = Instantiate(productionNodePrefab, productionNodeContainerObject);
                    SetUpProductionNode(productionNodeObject, ingredient.part, ingredient.amount * amountToCraft);
                    
                    CreateLineDrawer(ingredient.productionNode.GetComponent<RectTransform>(), productionNodeObject.GetComponent<RectTransform>());
                    
                    foreach (PartIngredient partIngredient in ingredient.part.activeRecipe.partIngredients)
                    {
                        partIngredient.productionNode = productionNodeObject;
                    }
                
                    if(nextTier.ContainsKey(ingredient.part))
                        nextTier[ingredient.part] += ingredient.amount * amountToCraft;
                    else
                        nextTier.Add(ingredient.part, ingredient.amount * amountToCraft);
                }

                foreach (MaterialIngredient ingredient in recipe.materialIngredients)
                {
                    if (!costMaterials.TryAdd(ingredient.material, ingredient.amount * amountToCraft))
                        costMaterials[ingredient.material] += ingredient.amount * amountToCraft;
                }
                
                if(nextTier.Count > 0)
                    tiers.Add(nextTier);

                tiers.Remove(thisTier);
            }
        }
        
        RectTransform productionNode = tProductionNodeObject.GetComponent<RectTransform>();
        RectTransform productionBranch = productionBranchObject.GetComponent<RectTransform>();
        
        //productionNode.rect.Set(productionNode.rect.x, productionNode.rect.y, productionNode.rect.width, productionBranch.rect.height);

        if (can)
        {
            StartCoroutine(SetBranchBaseHeightCo(productionNode, productionBranch));
            can = false;
        }
    }

    private void SetUpProductionNode(ProductionNode pNode, Part pPart, float pAmount)
    {
        pNode.icon.sprite = pPart.activeRecipe.productionIcon;
        pNode.partIcon.sprite = pPart.icon;
        pNode.productionPerMinute.text = pPart.activeRecipe.productionPerMinute + " per min";
        pNode.quantity.text = (pAmount / pPart.activeRecipe.productionPerMinute).ToString("0.00") + "x";
    }

    private void CreateLineDrawer(RectTransform from, RectTransform to)
    {
        LineDrawer line = Instantiate(lineDrawerPrefab, from);
        line.width = 0.2f;
        line.DrawLine(from, to);
        
        lines.Add(line.gameObject);
    }

    private IEnumerator SetBranchBaseHeightCo(RectTransform baseRect, RectTransform branchRect)
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < productionBranchBases.Count; i++)
        {
            RectTransform a = productionBranchBases[i].GetComponent<RectTransform>();
            RectTransform b = recipeTiersUI[i].GetComponent<RectTransform>();
            
            a.sizeDelta = new Vector2(a.rect.width, b.rect.height);
        }
    }
    
    private void ClearRecipe()
    {
        float count = recipeTiersUI.Count;
        for (int i = 0; i < count; i++)
            Destroy(recipeTiersUI[i].gameObject);
        recipeTiersUI.Clear();
        
        float costCount = costs.Count;
        for(int i = 0; i < costCount; i++)
            Destroy(costs[i].gameObject);
        costs.Clear();

        float lineCount = lines.Count;
        for(int i = 0; i < lineCount; i++)
            Destroy(lines[i].gameObject);
        lines.Clear();

        float branchBasesCount = productionBranchBases.Count;
        for (int i = 0; i < branchBasesCount; i++)
            Destroy(productionBranchBases[i].gameObject);
        productionBranchBases.Clear();
    }
    
    public void OnAmountChanged()
    {
        if (costs.Count <= 0)
            return;
        
        if (amountText.text == "")
        {
            amountText.text = "1";
            amountToCraft = 1;
        }
        
        amountToCraft = float.Parse(amountText.text);
        GenerateRecipe(outputPart);
    }
}