using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeNode<TKey, TValue>
{
    public TKey Key { get; set; }
    public TValue Value { get; set; }

    public TreeNode<TKey, TValue> Left { get; set; }
    public TreeNode<TKey, TValue> Right { get; set; }

    public int Height { get; set; }

    public TreeNode(TKey key, TValue value)
    {
        Key = key;
        Value = value;
        Left = null;
        Right = null;
        Height = 1;
    }
}
