using UnityEngine;
using UnityEngine.UI;

public class OutputMenuItem : MonoBehaviour
{
    public OutputSelector outputSelector;
    private Part part;
    
    [SerializeField] private Image icon;

    public void SetPart(Part newPart)
    {
        part = newPart;
        icon.sprite = part.icon;
    }

    public void OnSelect()
    {
        outputSelector.SelectOutputPart(part);
    }
}
