using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class PartIngredient
{
    public Part part;
    public float amount;

    public ProductionNode productionNode;
}

