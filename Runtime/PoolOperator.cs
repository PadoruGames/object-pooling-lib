using Padoru.Diagnostics;
using System;
using System.Collections.Generic;

namespace Padoru.ObjectPooling
{
    public class PoolOperator<T> where T : class
    {
        public T GetObject(string poolName, List<T> poolObjects, List<T> usedObjects, int maxPoolCapacity, Func<T> createObjectCallback)
        {
            if(poolObjects == null || usedObjects == null)
            {
                throw new Exception($"PoolObjects or UsedObjects list null {poolName}.");
            }

            if (poolObjects.Count <= 0)
            {
                if (HasCapacity(poolObjects, usedObjects, maxPoolCapacity))
                {
                    Debug.LogWarning($"Added new object to pool {poolName}.");

                    var newObj = createObjectCallback?.Invoke();
                    if(newObj == null)
                    {
                        throw new Exception($"CreateObjectCallback method is returning null in pool {poolName}.");
                    }

                    poolObjects.Add(newObj);
                }
                else
                {
                    throw new Exception($"Pool capacity reach {poolName}.");
                }
            }

            var obj = poolObjects[0];
            if (obj == null)
            {
                poolObjects.RemoveAt(0);
                throw new Exception($"Null object in pool {poolName}. Object removed from pool");
            }

            poolObjects.RemoveAt(0);
            usedObjects.Add(obj);

            return obj;
        }

        public void ReturnObject(T obj, string poolName, List<T> poolObjects, List<T> usedObjects)
        {
            if (poolObjects == null || usedObjects == null)
            {
                throw new Exception($"PoolObjects or UsedObjects list null {poolName}.");
            }

            if (obj == null)
            {
                throw new Exception($"Cannot return a null object to pool {poolName}");
            }

            if (poolObjects.Contains(obj))
            {
                return;
            }

            if (!usedObjects.Contains(obj))
            {
                throw new Exception($"The given object {obj} does not belong to this pool {poolName}");
            }

            usedObjects.Remove(obj);
            poolObjects.Add(obj);
        }

        private bool HasCapacity(List<T> poolObjects, List<T> usedObjects, int maxPoolCapacity)
        {
            return poolObjects.Count + usedObjects.Count < maxPoolCapacity;
        }
    }
}