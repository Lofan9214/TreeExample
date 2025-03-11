using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpacingTypes { Pow, LevelOrder, InOrder }


public class TreeVisualizer : MonoBehaviour
{
    public NodeVisualizer nodePrefab;

    public SpacingTypes spacingType = SpacingTypes.Pow;

    public float verticalSpacing = 1.5f;
    public float horizontalSpacing = 1.5f;

    private Dictionary<object, Vector3> positions = new Dictionary<object, Vector3>();

    private NodeVisualizer CreateNode<TKey, TValue>(TreeNode<TKey, TValue> node, Vector3 position) where TKey : IComparable<TKey>
    {
        var vNode = Instantiate(nodePrefab, position, Quaternion.identity, transform);
        vNode.SetNode(node);
        return vNode;
    }

    private NodeVisualizer CreateNode<TKey, TValue>(TreeNode<TKey, TValue> node) where TKey : IComparable<TKey>
    {
        if (node == null)
        {
            return null;
        }

        var pos = positions[node];
        var vNode = CreateNode(node, pos);

        var lNode = CreateNode(node.Left);
        if (lNode != null)
        {
            vNode.SetLeftLineEdge(lNode.transform.position);
        }
        var rNode = CreateNode(node.Right);
        if (rNode != null)
        {
            vNode.SetRightLineEdge(rNode.transform.position);
        }
        return vNode;
    }

    private void Clear()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void VisualizeTree<TKey, TValue>(BSTRoot<TKey, TValue> tree) where TKey : IComparable<TKey>
    {
        Clear();

        var root = tree.GetRoot();

        if (root == null)
        {
            return;
        }

        positions.Clear();

        switch (spacingType)
        {
            case SpacingTypes.Pow:
                VisualizeNode(root, Vector3.zero, root.Height - 1);
                break;
            case SpacingTypes.LevelOrder:
                AssignPositionsLevelOrder(root);
                CreateNode(root);
                break;
            case SpacingTypes.InOrder:
                int currentIndex = 0;
                AssignPositionInOrder(root, 0, ref currentIndex);
                CreateNode(root);
                break;
        }
    }

    private NodeVisualizer VisualizeNode<TKey, TValue>(TreeNode<TKey, TValue> node, Vector3 position, int height) where TKey : IComparable<TKey>
    {
        if (node == null)
        {
            return null;
        }

        var vNode = CreateNode(node, position);

        var basePosition = position + new Vector3(0f, -verticalSpacing, 0f);

        var leftPosition = basePosition;
        var rightPosition = basePosition;

        var offset = horizontalSpacing * Mathf.Pow(2, height - 1) * 0.5f;
        leftPosition.x -= offset;
        rightPosition.x += offset;

        var vNodeLeft = VisualizeNode(node.Left, leftPosition, height - 1);
        if (vNodeLeft != null)
        {
            vNode.SetLeftLineEdge(leftPosition);
        }

        var vNodeRight = VisualizeNode(node.Right, rightPosition, height - 1);
        if (vNodeRight != null)
        {
            vNode.SetRightLineEdge(rightPosition);
        }

        return vNode;
    }

    private void AssignPositionInOrder<TKey, TValue>(TreeNode<TKey, TValue> node, int depth, ref int currentIndex) where TKey : IComparable<TKey>
    {
        if (node == null)
        {
            return;
        }

        AssignPositionInOrder(node.Left, depth + 1, ref currentIndex);

        float x = currentIndex * horizontalSpacing;
        float y = -depth * verticalSpacing;
        positions[node] = new Vector3(x, y);

        ++currentIndex;

        AssignPositionInOrder(node.Right, depth + 1, ref currentIndex);
    }

    private void AssignPositionsLevelOrder<TKey, TValue>(TreeNode<TKey, TValue> node) where TKey : IComparable<TKey>
    {
        var queue = new Queue<(TreeNode<TKey, TValue>, int)>();
        queue.Enqueue((node, 0));

        var levels = new List<List<TreeNode<TKey, TValue>>>();

        while (queue.Count > 0)
        {
            var (currentNode, level) = queue.Dequeue();

            if (levels.Count <= level)
            {
                levels.Add(new List<TreeNode<TKey, TValue>>());
            }

            levels[level].Add(currentNode);

            if (currentNode.Left != null)
            {
                queue.Enqueue((currentNode.Left, level + 1));
            }

            if (currentNode.Right != null)
            {
                queue.Enqueue((currentNode.Right, level + 1));
            }
        }

        for (int i = 0; i < levels.Count; ++i)
        {
            var levelNodes = levels[i];
            int count = levelNodes.Count;

            float totalWidth = (count - 1) * horizontalSpacing;
            float startX = -totalWidth * 0.5f;
            float y = i * -verticalSpacing;

            for (int j = 0; j < count; ++j)
            {
                float x = startX + (j * horizontalSpacing);
                positions[levelNodes[j]] = new Vector3(x, y);
            }
        }
    }
}