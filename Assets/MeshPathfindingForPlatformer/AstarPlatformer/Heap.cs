﻿using System;

namespace Calcatz.MeshPathfinding {

    public class Heap<T> where T : IHeapItem<T> {

        private readonly T[] _items;
        private int _currentItemCount;

        public Heap(int maxSize) {
            _items = new T[maxSize];
        }

        public void Add(T item) {
            item.HeapIndex = _currentItemCount;
            _items[_currentItemCount] = item;
            SortUp(item);
            _currentItemCount++;
        }

        void SortUp(T item) {
            int parentIndex = (item.HeapIndex - 1) / 2;
            while (true) {
                T parentItem = _items[parentIndex];
                if (item.CompareTo(parentItem) > 0) {
                    Swap(item, parentItem);
                } else {
                    break;
                }
                parentIndex = (item.HeapIndex - 1) / 2;
            }
        }

        void SortDown(T item) {
            while (true) {
                int childIndexLeft = item.HeapIndex * 2 + 1;
                int childIndexRight = item.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < _currentItemCount) {
                    swapIndex = childIndexLeft;
                    if (childIndexRight < _currentItemCount) {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0) {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (item.CompareTo(_items[swapIndex]) < 0) {
                        Swap(item, _items[swapIndex]);
                    } else {
                        return;
                    }
                } else {
                    return;
                }
            }
        }

        void Swap(T item1, T item2) {
            _items[item1.HeapIndex] = item2;
            _items[item2.HeapIndex] = item1;
            int tempIndex = item1.HeapIndex;
            item1.HeapIndex = item2.HeapIndex;
            item2.HeapIndex = tempIndex;
        }

        public T RemoveFirstItem() {
            T firstItem = _items[0];
            _currentItemCount--;
            _items[0] = _items[_currentItemCount];
            _items[0].HeapIndex = 0;
            SortDown(_items[0]);
            return firstItem;
        }

        public void UpdateItem(T item) {
            SortUp(item);
        }

        public int Count {
            get {
                return _currentItemCount;
            }
        }

        public bool Contains(T item) {
            return Equals(_items[item.HeapIndex], item);
        }
    }

    public interface IHeapItem<T> : IComparable<T> {
        int HeapIndex { get; set; }
    }
}