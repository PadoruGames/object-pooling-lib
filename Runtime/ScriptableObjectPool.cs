using System.Collections.Generic;
using UnityEngine;

namespace Padoru.ObjectPooling
{
    public abstract class ScriptableObjectPool<T> : ScriptableObject, IPool<T> where T : class
    {
        [SerializeField] private int startAmount;
        [SerializeField] private int maxCapacity;

        private List<T> poolObjects;
        private List<T> usedObjects;
        private PoolOperator<T> poolOperator;

        public void Init()
        {
            poolObjects = new List<T>();
            usedObjects = new List<T>();
            poolOperator = new PoolOperator<T>();

            for (int i = 0; i < startAmount; i++)
            {
                var newObj = CreateObject();
                poolObjects.Add(newObj);
            }

            OnInitialization();
        }

        public virtual T GetObject()
        {
            var obj = poolOperator.GetObject(name, poolObjects, usedObjects, maxCapacity, CreateObject);

            if(obj != null)
            {
                OnGetObject(obj);
            }

            return obj;
        }

        public virtual void ReturnObject(T obj)
        {
            if (obj != null)
            {
                OnReturnObject(obj);
            }

            poolOperator.ReturnObject(obj, name, poolObjects, usedObjects);
        }

        protected abstract T CreateObject();

        protected virtual void OnInitialization() { }

        protected virtual void OnGetObject(T obj) { }

        protected virtual void OnReturnObject(T obj) { }
    }
}