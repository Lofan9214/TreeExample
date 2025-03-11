using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VisualizationTree : MonoBehaviour
{
    private BinarySearchTree<int, int> binaryTree = new BinarySearchTree<int, int>();
    private AVLTree<int, int> avlTree = new AVLTree<int, int>();
    private BinarySearchTree<int, VisualizationNode> visualizationTree = new BinarySearchTree<int, VisualizationNode>();

    [SerializeField]
    private VisualizationNode prefab;

    [SerializeField]
    private int keyMin = 0;
    [SerializeField]
    private int keyMax = 1000;

    [SerializeField]
    private int valueMin = 100;
    [SerializeField]
    private int valueMax = 300;

    [SerializeField]
    private int nodeMaxCount = 3;

    public void InitAVL()
    {
        InitializeTree(avlTree);

        InitializeVisualTree(avlTree.Root);

        var reverse = avlTree.LevelTraversalNode().Reverse();
        var preOrder = avlTree.PreOrderTraversalNode();

        UpdateVisualNode(reverse, preOrder);
    }

    public void InitBin()
    {
        InitializeTree(binaryTree);

        InitializeVisualTree(binaryTree.Root);

        var reverse = binaryTree.LevelTraversalNode().Reverse();
        var preOrder = binaryTree.PreOrderTraversalNode();

        UpdateVisualNode(reverse, preOrder);
    }

    private void InitializeTree(BinarySearchTree<int, int> tree)
    {
        tree.Clear();

        int count = 0;
        while (count < nodeMaxCount)
        {
            int randKey;
            do
            {
                randKey = Random.Range(keyMin, keyMax);
            }
            while (tree.ContainsKey(randKey));

            tree.Add(randKey, Random.Range(valueMin, valueMax));

            ++count;
        }
    }

    private void InitializeVisualTree(TreeNode<int, int> node)
    {
        if (visualizationTree.Root != null)
            Destroy(visualizationTree.Root.Value.gameObject);
        visualizationTree.Clear();

        InstantiateVisualNode(node);
    }

    private void InstantiateVisualNode(TreeNode<int, int> node, Transform parent = null)
    {
        if (node != null)
        {
            var nodeGO = Instantiate(prefab);
            nodeGO.Key = node.Key;
            nodeGO.Value = node.Value;
            nodeGO.Height = node.Height;
            nodeGO.transform.parent = parent;

            visualizationTree.Add(node.Key, nodeGO);

            InstantiateVisualNode(node.Left, nodeGO.transform);
            InstantiateVisualNode(node.Right, nodeGO.transform);
        }
    }

    public void UpdateVisualNode(IEnumerable<TreeNode<int, int>> levelReverse, IEnumerable<TreeNode<int, int>> PreOrder)
    {
        foreach (var node in levelReverse)
        {
            if (node != null)
            {
                if (node.Left != null)
                {
                    visualizationTree[node.Key].LeftWidth = visualizationTree[node.Left.Key].FullWidth;
                }
                else
                {
                    visualizationTree[node.Key].LeftWidth = 10f;
                }
                if (node.Right != null)
                {
                    visualizationTree[node.Key].RightWidth = visualizationTree[node.Right.Key].FullWidth;
                }
                else
                {
                    visualizationTree[node.Key].RightWidth = 10f;
                }
            }
        }

        foreach (var node in PreOrder)
        {
            LineRenderer thisline = visualizationTree[node.Key].GetComponent<LineRenderer>();
            if (node.Left != null)
            {
                visualizationTree[node.Left.Key].transform.localPosition =
                     new Vector2(-visualizationTree[node.Left.Key].RightWidth, -10f);

                int i = thisline.positionCount;
                thisline.positionCount += 2;
                thisline.SetPosition(i, visualizationTree[node.Left.Key].transform.position);
                thisline.SetPosition(i + 1, visualizationTree[node.Key].transform.position);
            }
            if (node.Right != null)
            {
                visualizationTree[node.Right.Key].transform.localPosition =
                    new Vector2(visualizationTree[node.Right.Key].LeftWidth, -10f);

                int i = thisline.positionCount;
                thisline.positionCount += 2;
                thisline.SetPosition(i, visualizationTree[node.Right.Key].transform.position);
                thisline.SetPosition(i + 1, visualizationTree[node.Key].transform.position);
            }
        }
    }
}
