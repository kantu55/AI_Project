using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    //  コンストラクタ
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // ノードの追加
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    // 親ノードよりも優先度が高い場合は交換する
    void SortUp(T item)
    {
        // 現在のノードの親ノードを探す
        int parentIndex = (item.HeapIndex - 1) / 2;
        while (true)
        {
            T parentItem = items[parentIndex];
            // Fコストが親よりも高かったら交換
            // CompareTo・・・引数と比較して引数より高ければ1、低ければ-1を返す
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            // 交換後、交換された親ノードを探す
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // ノードを削除する
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    //子ノードと比較して順番を変える
    void SortDown(T item)
    {
        // 交換するノードを左か右かを選ぶ
        while(true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            // 子ノード（左側）がいるか確認
            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;
                // 子ノード（右側）がいるか確認
                if(childIndexRight < currentItemCount)
                {
                    // ２つの子ノードの内どちらの優先度が高いか比較
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    // ノードの交換
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }

    // 特定のノードが含まれているか
    public bool Contains(T item)
    {
        // Equals・・・２つの引数の値が等しいか
        return Equals(items[item.HeapIndex], item);
    }

    // ソートをしてノードを更新
     public void UpdateItem(T item)
    {
        SortUp(item);
    }

     // ノードの数を取得する 
     public int Count
    {
        get
        {
            return currentItemCount;
        }
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}
