using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTree<TKey, TValue> : BinarySearchTree<TKey, TValue> where TKey : IComparable<TKey>
{
    protected override TreeNode<TKey, TValue> Add(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        return Balance(base.Add(node, key, value));
    }

    protected override TreeNode<TKey, TValue> AddOrUpdate(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        return Balance(base.AddOrUpdate(node, key, value));
    }

    protected override TreeNode<TKey, TValue> Remove(TreeNode<TKey, TValue> node, TKey key)
    {
        return Balance(base.Remove(node, key));
    }

    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        int diff = BalanceFactor(node);

        if (diff < -1)
        {
            int diff2 = BalanceFactor(node.Right);
            if (diff2 > 0)
            {
                node.Right = RightRotation(node.Right);
            }
            node = LeftRotation(node);
            return node;
        }
        else if (diff > 1)
        {
            int diff2 = BalanceFactor(node.Left);
            if (diff2 < 0)
            {
                node.Left = LeftRotation(node.Left);
            }
            node = RightRotation(node);
            return node;
        }
        else
        {
            return node;
        }
    }

    protected TreeNode<TKey, TValue> RightRotation(TreeNode<TKey, TValue> node)
    {
        var temp = node.Left;
        node.Left = temp.Right;
        temp.Right = node;
        UpdateHeight(node);
        UpdateHeight(temp);
        return temp;
    }

    protected TreeNode<TKey, TValue> LeftRotation(TreeNode<TKey, TValue> node)
    {
        var temp = node.Right;
        node.Right = temp.Left;
        temp.Left = node;
        UpdateHeight(node);
        UpdateHeight(temp);
        return temp;
    }

    protected virtual int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        int left = node.Left == null ? 0 : node.Left.Height;
        int right = node.Right == null ? 0 : node.Right.Height;
        int diff = left - right;
        return diff;
    }
}
