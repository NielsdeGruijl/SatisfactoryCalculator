using System.Collections;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public LineRenderer lineRenderer;

    public float width;
    
    public void DrawLine(RectTransform from, RectTransform to)
    {
        lineRenderer.startWidth = lineRenderer.endWidth = width;
        
        StartCoroutine(DrawLineCo(from, to));
    }
    
    IEnumerator DrawLineCo(RectTransform from, RectTransform to)
    {
        float duration = 0.5f;
        float timeElapsed = 0;
        
        while(duration > timeElapsed)
        {
            lineRenderer.SetPosition(0, from.position - new Vector3(0, 0, -1));
            lineRenderer.SetPosition(1, to.position - new Vector3(0, 0, -1));
            timeElapsed += Time.deltaTime;
            yield return null;
        }

    }
}
