using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NodeVisualizer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMeshPro text;
    public LineRenderer leftLineRenderer;
    public LineRenderer rightLineRenderer;

    public void SetNode<TKey, TValue>(TreeNode<TKey, TValue> node)
    {
        text.text = $"K: {node.Key}\nV: {node.Value}\nH: {node.Height}";
    }

    public void SetLeftLineEdge(Vector3 position)
    {
        leftLineRenderer.positionCount = 2;
        leftLineRenderer.SetPosition(0, transform.position);
        leftLineRenderer.SetPosition(1, position);
    }

    public void SetRightLineEdge(Vector3 position)
    {
        rightLineRenderer.positionCount = 2;
        rightLineRenderer.SetPosition(0, transform.position);
        rightLineRenderer.SetPosition(1, position);
    }
}
