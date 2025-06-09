using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialNode : MonoBehaviour
{
    public Material material;

    public Image icon;
    public TMP_Text amount;

    public void SetMaterial(MaterialIngredient materialIngredient)
    {
        material = materialIngredient.material;
        icon.sprite = material.icon;
        amount.text = materialIngredient.amount.ToString("0.0");
    }
}
