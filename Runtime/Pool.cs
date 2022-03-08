using System;
using System.Collections.Generic;

namespace Padoru.ObjectPooling
{
    public class Pool<T> : IPool<T> where T : class
    {
        private string name;
        private int startAmount;
        private int maxCapacity;

        private List<T> poolObjects;
        private List<T> usedObjects;
        private PoolOperator<T> poolOperator;

        private Func<T> createObjectCallback;
        
        public Pool(string name, int startAmount, int maxCapacity, Func<T> createObjectCallback)
        {
            this.name = name;
            this.startAmount = startAmount;
            this.maxCapacity = maxCapacity;
            this.createObjectCallback = createObjectCallback;

            poolObjects = new List<T>(maxCapacity);
            usedObjects = new List<T>(maxCapacity);
            poolOperator = new PoolOperator<T>();

            for (int i = 0; i < this.startAmount; i++)
            {
                var newObj = createObjectCallback?.Invoke();
                poolObjects.Add(newObj);
            }
        }

        public T GetObject()
        {
            return poolOperator.GetObject(name, poolObjects, usedObjects, maxCapacity, createObjectCallback);
        }

        public void ReturnObject(T obj)
        {
            poolOperator.ReturnObject(obj, name, poolObjects, usedObjects);
        }
    }
}
