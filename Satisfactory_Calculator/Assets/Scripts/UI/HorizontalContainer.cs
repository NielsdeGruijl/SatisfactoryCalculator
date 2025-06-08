using System.Collections.Generic;
using UnityEngine;

public class HorizontalContainer : MonoBehaviour
{
    public Transform content;
    //public float itemWidth;
    
    private List<Transform> children = new List<Transform>();
    private int childCount;

    private float contentPos = 0.5f;

    private void Update()
    {
        if (content.childCount > childCount)
        {
            childCount++;
            AddChild(content.GetChild(childCount - 1));
        }
    }

    private void AddChild(Transform newChild)
    {
        children.Add(newChild);
        transform.localScale += new Vector3(newChild.localScale.x, 0, 0);
        content.localScale = new Vector3(1 / transform.localScale.x, 1, 1);
        content.localPosition = new Vector3(contentPos / transform.localScale.x, 0, 0);

        float displacement = 0;
        
        for (int i = 1; i < children.Count; i++)
        {
            displacement += children[i].localScale.x;
        }
        
        newChild.localPosition = new Vector3(displacement + newChild.localPosition.x / 2, 0, 0);
    }
}
