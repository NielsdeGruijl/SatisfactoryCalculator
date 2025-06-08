using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionNode : MonoBehaviour
{
    public Part part;
    public Transform parent;
    
    public Image icon;
    public Image partIcon;
    public TMP_Text productionPerMinute;
    public TMP_Text quantity;
}
