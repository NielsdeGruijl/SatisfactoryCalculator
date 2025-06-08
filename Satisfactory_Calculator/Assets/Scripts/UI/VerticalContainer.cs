using System;
using System.Collections.Generic;
using UnityEngine;

public class VerticalContainer : MonoBehaviour
{
    public float itemHeight = 2;
    
    private List<Transform> children = new List<Transform>();
    
    private int childCount = 0;
    private float containerHeight;
    
    private void Update()
    {
        if (transform.childCount > childCount)
        {
            ChildAdded(transform.GetChild(transform.childCount - 1));
        }
    }

    private void ChildAdded(Transform newChild)
    {
        childCount++;
        children.Insert(Mathf.FloorToInt(children.Count / 2f), newChild);

        if (childCount > 1)
        { 
            // First half of children excluding middle if uneven amount
            int halfCount = Mathf.FloorToInt(children.Count / 2f);
            
            for (int i = halfCount; i > 0 ; i--)
            {
                int index = i - 1;
                if (childCount % 2 == 0)
                {
                    children[index].localPosition = new Vector3(0, itemHeight / 2 + itemHeight * (halfCount - i), 0);
                }
                else
                {
                    children[index].localPosition = new Vector3(0, itemHeight + itemHeight * (halfCount - i), 0);
                }
            }
            
            // Second half of children excluding middle if uneven amount
            halfCount = Mathf.CeilToInt(children.Count / 2f);
            
            for (int i = halfCount; i < children.Count; i++)
            {
                if (childCount % 2 == 0)
                {
                    children[i].localPosition = new Vector3(0, (itemHeight / 2 + itemHeight * (i - halfCount)) * -1, 0);
                }
                else
                {
                    children[i].localPosition = new Vector3(0, (itemHeight + itemHeight * (i - halfCount)) * -1, 0);
                }
            }
        }
    }
}
