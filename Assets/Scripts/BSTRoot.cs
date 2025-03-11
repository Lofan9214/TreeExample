using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BSTRoot<TKey, TValue> : BinarySearchTree<TKey,TValue> where TKey : IComparable<TKey>
{
    public TreeNode<TKey,TValue> GetRoot()
    {
        return root;
    }
}
