using System.Collections.Generic;
using UnityEngine;

namespace Padoru.ObjectPooling
{
    public abstract class ScriptableObjectPool<T> : ScriptableObject, IPool<T> where T : class
    {
        [SerializeField] private int startAmount;
        [SerializeField] private int maxCapacity;

        private List<T> poolObjects = new List<T>();
        private List<T> usedObjects = new List<T>();
        private PoolOperator<T> poolOperator = new PoolOperator<T>();

        protected virtual void Awake()
        {
            if (!Application.isPlaying)
            {
                return;
            }

            for (int i = 0; i < startAmount; i++)
            {
                var newObj = CreateObject();
                poolObjects.Add(newObj);
            }
        }

        public virtual T GetObject()
        {
            var obj = poolOperator.GetObject(name, poolObjects, usedObjects, maxCapacity, CreateObject);
            OnGetObject(obj);
            return obj;
        }

        public virtual void ReturnObject(T obj)
        {
            OnReturnObject(obj);
            poolOperator.ReturnObject(obj, name, poolObjects, usedObjects);
        }

        protected abstract T CreateObject();

        protected virtual void OnGetObject(T obj) { }

        protected virtual void OnReturnObject(T obj) { }
    }
}