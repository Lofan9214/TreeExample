using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AVLTreePractice<TKey, TValue> : BSTRoot<TKey, TValue> where TKey : IComparable<TKey>
{
    public AVLTreePractice() : base()
    {
    }

    protected override TreeNode<TKey, TValue> Add(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.Add(node, key, value);
        return Balance(node);
    }

    protected override TreeNode<TKey, TValue> AddOrUpdate(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        node = base.AddOrUpdate(node, key, value);
        return Balance(node);
    }

    protected override TreeNode<TKey, TValue> Remove(TreeNode<TKey, TValue> node, TKey key)
    {
        node = base.Remove(node, key);
        return Balance(node);
    }

    protected int BalanceFactor(TreeNode<TKey, TValue> node)
    {
        return (node == null) ? 0 : (node.Left != null ? node.Left.Height : 0) - (node.Right != null ? node.Right.Height : 0);
    }

    protected TreeNode<TKey, TValue> Balance(TreeNode<TKey, TValue> node)
    {
        int factor = BalanceFactor(node);
        if (factor > 1)
        {
            if (BalanceFactor(node.Left) < 0)
            {
                node.Left = RotateLeft(node.Left);
            }
            return RotateRight(node);
        }
        else if (factor < -1)
        {
            if (BalanceFactor(node.Right) > 0)
            {
                node.Right = RotateRight(node.Right);
            }
            return RotateLeft(node);
        }
        return node;

    }

    protected TreeNode<TKey, TValue> RotateLeft(TreeNode<TKey, TValue> node)
    {
        var rightChild = node.Right;
        var leftSubTreeOfRightChild = node.Right.Left;

        rightChild.Left = node;
        node.Right = leftSubTreeOfRightChild;

        UpdateHeight(node);
        UpdateHeight(rightChild);

        return rightChild;
    }

    protected TreeNode<TKey, TValue> RotateRight(TreeNode<TKey, TValue> node)
    {
        var leftChild = node.Left;
        var rightSubTreeOfLeftChild = node.Left.Right;

        leftChild.Right = node;
        node.Left = rightSubTreeOfLeftChild;

        UpdateHeight(node);
        UpdateHeight(leftChild);

        return leftChild;
    }

}
