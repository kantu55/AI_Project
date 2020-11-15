using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    /*
    * コンストラクタ
    * @param maxHeapSize グリッドの数
    */ 
    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    // アイテムの追加
    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    // ヒープからアイテムを削除する
    public T RemoveFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    // アイテムを取り込む
    void SortDown(T item)
    {
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

    // 特定のアイテムが含まれているか
    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    // アイテムの更新
     public void UpdateItem(T item)
    {
        SortUp(item);
    }

     // アイテムの数を取得する 
     public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    // 親ノードよりも優先度が高い場合は交換する
    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;
        while(true)
        {
            T parentItem = items[parentIndex];
            // Fコストが親よりも高かったら交換
            if(item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    // アイテムの交換
    void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
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
