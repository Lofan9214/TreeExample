using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BinarySearchTree<TKey, TValue> : IDictionary<TKey, TValue> where TKey : IComparable<TKey>
{
    protected TreeNode<TKey, TValue> root;

    public BinarySearchTree()
    {
        root = null;
    }

    public TValue this[TKey key]
    {
        get
        {
            if (TryGetValue(key, out TValue value))
            {
                return value;
            }
            throw new KeyNotFoundException($"{key} : 키 없음");
        }
        set
        {
            root = AddOrUpdate(root, key, value);
        }
    }

    public ICollection<TKey> Keys => InOrderTraversal().Select(kvp => kvp.Key).ToList();

    public ICollection<TValue> Values => InOrderTraversal().Select(kvp => kvp.Value).ToList();

    public int Count => CountNodes(root);

    protected virtual int CountNodes(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            return 0;
        }
        return 1 + CountNodes(node.Left) + CountNodes(node.Right);
    }

    public bool IsReadOnly => false;

    public void Add(TKey key, TValue value)
    {
        root = Add(root, key, value);
    }

    public void Add(KeyValuePair<TKey, TValue> item)
    {
        Add(item.Key, item.Value);
    }

    protected virtual TreeNode<TKey, TValue> Add(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        if (node == null)
        {
            return new TreeNode<TKey, TValue>(key, value);
        }

        int compare = key.CompareTo(node.Key);
        if (compare < 0)
        {
            node.Left = Add(node.Left, key, value);
        }
        else if (compare > 0)
        {
            node.Right = Add(node.Right, key, value);
        }
        else
        {
            throw new ArgumentException($"{key} 키 중복");
        }

        UpdateHeight(node);
        return node;
    }

    protected virtual TreeNode<TKey, TValue> AddOrUpdate(TreeNode<TKey, TValue> node, TKey key, TValue value)
    {
        if (node == null)
        {
            return new TreeNode<TKey, TValue>(key, value);
        }

        int compare = key.CompareTo(node.Key);
        if (compare < 0)
        {
            node.Left = AddOrUpdate(node.Left, key, value);
        }
        else if (compare > 0)
        {
            node.Right = AddOrUpdate(node.Right, key, value);
        }
        else
        {
            node.Value = value;
        }

        UpdateHeight(node);
        return node;
    }

    protected virtual void UpdateHeight(TreeNode<TKey, TValue> node)
    {
        node.Height = Mathf.Max(Height(node.Left), Height(node.Right)) + 1;
    }

    protected virtual int Height(TreeNode<TKey, TValue> node)
    {
        return node == null ? 0 : node.Height;
    }

    public void Clear()
    {
        root = null;
    }

    public bool Contains(KeyValuePair<TKey, TValue> item)
    {
        return ContainsKey(item.Key);
    }

    public bool ContainsKey(TKey key)
    {
        return TryGetValue(key, out _);
    }

    public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        foreach (var kvp in this)
        {
            array[arrayIndex++] = kvp;
        }
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return InOrderTraversal().GetEnumerator();
    }

    public bool Remove(TKey key)
    {
        int initialCount = Count;
        root = Remove(root, key);
        return Count < initialCount;
    }

    public bool Remove(KeyValuePair<TKey, TValue> item)
    {
        return Remove(item.Key);
    }

    protected virtual TreeNode<TKey, TValue> Remove(TreeNode<TKey, TValue> node, TKey key)
    {
        if (node == null)
        {
            return node;
        }

        int compare = key.CompareTo(node.Key);

        if (compare < 0)
        {
            node.Left = Remove(node.Left, key);
        }
        else if (compare > 0)
        {
            node.Right = Remove(node.Right, key);
        }
        else
        {
            if (node.Left == null)
            {
                return node.Right;
            }
            else if (node.Right == null)
            {
                return node.Left;
            }

            TreeNode<TKey, TValue> minNode = FindMin(node.Right);
            node.Key = minNode.Key;
            node.Value = minNode.Value;
            node.Right = Remove(node.Right, minNode.Key);
        }

        UpdateHeight(node);
        return node;
    }

    protected virtual TreeNode<TKey, TValue> FindMin(TreeNode<TKey, TValue> node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return TryGetValue(root, key, out value);
    }

    protected bool TryGetValue(TreeNode<TKey, TValue> node, TKey key, out TValue value)
    {
        if (node == null)
        {
            value = default(TValue);
            return false;
        }

        int compare = key.CompareTo(node.Key);

        if (compare == 0)
        {
            value = node.Value;
            return true;
        }
        else if (compare < 0)
        {
            return TryGetValue(node.Left, key, out value);
        }
        else
        {
            return TryGetValue(node.Right, key, out value);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public virtual IEnumerable<KeyValuePair<TKey, TValue>> InOrderTraversal()
    {
        return InOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> InOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            foreach (var kvp in InOrderTraversal(node.Left))
            {
                yield return kvp;
            }
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            foreach (var kvp in InOrderTraversal(node.Right))
            {
                yield return kvp;
            }
        }
    }

    public virtual IEnumerable<KeyValuePair<TKey, TValue>> PreOrderTraversal()
    {
        return PreOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> PreOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
            foreach (var kvp in PreOrderTraversal(node.Left))
            {
                yield return kvp;
            }
            foreach (var kvp in PreOrderTraversal(node.Right))
            {
                yield return kvp;
            }
        }
    }

    public virtual IEnumerable<KeyValuePair<TKey, TValue>> PostOrderTraversal()
    {
        return PostOrderTraversal(root);
    }

    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> PostOrderTraversal(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            foreach (var kvp in PostOrderTraversal(node.Left))
            {
                yield return kvp;
            }
            foreach (var kvp in PostOrderTraversal(node.Right))
            {
                yield return kvp;
            }
            yield return new KeyValuePair<TKey, TValue>(node.Key, node.Value);
        }
    }
    public virtual IEnumerable<KeyValuePair<TKey, TValue>> LevelTraversal()
    {
        return LevelTraversal(root);
    }

    //protected virtual IEnumerable<KeyValuePair<TKey, TValue>> LevelTraversal(TreeNode<TKey, TValue> node)
    //{
    //    if (node != null)
    //    {
    //        Queue<TreeNode<TKey, TValue>> queue = new Queue<TreeNode<TKey, TValue>>();

    //        queue.Enqueue(node);
    //        while (queue.Count > 0)
    //        {
    //            var item = queue.Dequeue();
    //            yield return new KeyValuePair<TKey, TValue>(item.Key, item.Value);
    //            if (item.Left != null)
    //            {
    //                queue.Enqueue(item.Left);
    //            }
    //            if (item.Right != null)
    //            {
    //                queue.Enqueue(item.Right);
    //            }
    //        }
    //    }
    //}

    protected virtual IEnumerable<KeyValuePair<TKey, TValue>> LevelTraversal(TreeNode<TKey, TValue> node)
    {
        if (node == null)
        {
            yield break;
        }

        var queue = new Queue<TreeNode<TKey, TValue>>();

        queue.Enqueue(node);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            yield return new KeyValuePair<TKey, TValue>(current.Key, current.Value);

            if (current.Left != null)
            {
                queue.Enqueue(current.Left);
            }
            if (current.Right != null)
            {
                queue.Enqueue(current.Right);
            }
        }
    }

    public IEnumerable<TreeNode<TKey, TValue>> PreOrderTraversalNode()
    {
        return PreOrderTraversalNode(root);
    }

    protected IEnumerable<TreeNode<TKey, TValue>> PreOrderTraversalNode(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            yield return node;
            foreach (var subNode in PreOrderTraversalNode(node.Left))
            {
                yield return subNode;
            }
            foreach (var subNode in PreOrderTraversalNode(node.Right))
            {
                yield return subNode;
            }
        }
    }

    public IEnumerable<TreeNode<TKey, TValue>> PostOrderTraversalNode()
    {
        return PostOrderTraversalNode(root);
    }

    protected IEnumerable<TreeNode<TKey, TValue>> PostOrderTraversalNode(TreeNode<TKey, TValue> node)
    {
        if (node != null)
        {
            foreach (var subNode in PostOrderTraversalNode(node.Left))
            {
                yield return subNode;
            }
            foreach (var subNode in PostOrderTraversalNode(node.Right))
            {
                yield return subNode;
            }
            yield return node;
        }
    }

    public virtual IEnumerable<TreeNode<TKey, TValue>> LevelTraversalNode()
    {
        if (root != null)
        {
            Queue<TreeNode<TKey, TValue>> queue = new Queue<TreeNode<TKey, TValue>>();

            queue.Enqueue(root);
            while (queue.Count > 0)
            {
                var item = queue.Dequeue();
                yield return item;
                if (item.Left != null)
                {
                    queue.Enqueue(item.Left);
                }
                if (item.Right != null)
                {
                    queue.Enqueue(item.Right);
                }
            }
        }
    }

    public virtual TreeNode<TKey, TValue> Root => root;
}
