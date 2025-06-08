using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PartList", menuName = "ScriptableObjects/PartList")]
public class PartListSO : ScriptableObject
{
    public List<Part> parts;
}
