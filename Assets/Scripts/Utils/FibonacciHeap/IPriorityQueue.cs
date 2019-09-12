using System;
using System.Collections.Generic;
using UnityEngine;

public interface IPriorityQueue<TKey, TValue> where TKey : IComparable<TKey>
{
    int Size { get; }
    bool IsEmpty { get; }

    void Clear();
    void DecreaseKey(INode<TKey, TValue> node, TKey newKey);
    void Delete(INode<TKey, TValue> node);
    INode<TKey, TValue> ExtractMinimum();
    INode<TKey, TValue> FindMinimum();
    INode<TKey, TValue> Insert(TKey key, TValue val);
    void Union(IPriorityQueue<TKey, TValue> other);
}

public interface INode<TKey, TValue> : IComparable
      where TKey : IComparable<TKey>
{
    TKey Key { get; }
    TValue Value { get; }
}