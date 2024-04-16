using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableOutline : MonoBehaviour
{
    public LineRenderer outlineRenderer;

    void Start()
    {
        if (outlineRenderer == null)
        {
            outlineRenderer = GetComponent<LineRenderer>();
            outlineRenderer.startWidth = 0.05f;
            outlineRenderer.endWidth = 0.05f;
            outlineRenderer.material = new Material(Shader.Find("Unlit/Color"));
            outlineRenderer.material.color = Color.green;
        }

        DisableOutline();
    }

    public void EnableOutline()
    {
        outlineRenderer.enabled = true;
    }

    public void DisableOutline()
    {
        outlineRenderer.enabled = false;
    }
}
