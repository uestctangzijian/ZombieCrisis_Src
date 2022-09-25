using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextLogEntry
{
    public TextMeshProUGUI TextMesh { get; set; }
    public int Index { get; set; }

    public TextLogEntry(TextMeshProUGUI textMesh, int index, Color color)
    {
        this.TextMesh = textMesh;
        this.Index = index;
        this.TextMesh.color = color;
    }

    public void SetText(string text) => TextMesh.text = text;
    
    public void MoveToIndex(int index)
    {
        Index = index;
        TextMesh.transform.SetSiblingIndex(index);
    }

    public void Destroy()
    {
        GameObject.Destroy(TextMesh.gameObject);
    }
}
