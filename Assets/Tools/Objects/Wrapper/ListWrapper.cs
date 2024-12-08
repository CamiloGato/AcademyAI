using System.Collections.Generic;
using UnityEngine;

namespace Tools.Objects.Wrapper
{
    public class ListWrapper<T> : ScriptableObject {
        public List<T> elements = new ();
        public int Count => elements.Count;
        public T this[int index] => elements[index];
        public void Add(T element) => elements.Add(element);
        public void Remove(T element) => elements.Remove(element);
        public void RemoveAt(int index) => elements.RemoveAt(index);
        public void Clear() => elements.Clear();
    }
}