using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PartLoader : MonoBehaviour
{
    public Part[] parts;
    
    void Awake()
    {
        string[] partPaths = AssetDatabase.FindAssets("t:Part");
        parts = new Part[partPaths.Length];

        for (int i = 0; i < partPaths.Length; i++)
        {
            var path = AssetDatabase.GUIDToAssetPath(partPaths[i]);
            parts[i] =  AssetDatabase.LoadAssetAtPath<Part>(path);
        }
    }
}
