using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public TreeVisualizer treeVisualizer;

    public int nodeCount = 20;
    public int minKey = 1;
    public int maxKey = 1000;

    private void Start()
    {
        GenerateRandomTree();
    }

    public void GenerateRandomTree()
    {
        var tree = new AVLTreePractice<int, string>();

        int count = 0;

        while (count < nodeCount)
        {
            int key = Random.Range(minKey, maxKey);

            if (!tree.ContainsKey(key))
            {
                string value = key.ToString();
                tree.Add(key, value);
                ++count;
            }
        }
        treeVisualizer.VisualizeTree(tree);
    }

    [ContextMenu("Reset Tree")]
    public void ResetTree()
    {
        GenerateRandomTree();
    }


    //void Start()
    //{
    //    var bst = new BinarySearchTree<string, string>();

    //    bst["17"] = "seventeen";
    //    bst["05"] = "five";
    //    bst["01"] = "one";
    //    bst["18"] = "eighteen";
    //    bst["08"] = "eight";
    //    bst["15"] = "fifteen";
    //    bst["12"] = "twelve";
    //    bst["00"] = "zero";

    //    foreach(var item in bst)
    //    {
    //        Debug.Log(item);
    //    }

    //    bst.Remove("05");

    //    foreach(var item in bst)
    //    {
    //        Debug.Log(item);
    //    }
    //}
}
