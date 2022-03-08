using Padoru.Diagnostics;
using System;
using System.Collections.Generic;

namespace Padoru.ObjectPooling
{
    internal class PoolOperator<T> where T : class
    {
        internal T GetObject(string poolName, List<T> poolObjects, List<T> usedObjects, int maxPoolCapacity, Func<T> createObjectCallback)
        {
            if (poolObjects.Count <= 0)
            {
                if (HasCapacity(poolObjects, usedObjects, maxPoolCapacity))
                {
                    Debug.LogWarning($"Added new object to pool {poolName}");
                    var newObj = createObjectCallback?.Invoke();
                    poolObjects.Add(newObj);
                }
                else
                {
                    Debug.LogError($"Pool capacity reach {poolName}");
                    return null;
                }
            }

            var obj = poolObjects[0];
            if (obj == null)
            {
                Debug.LogError($"Null object in pool {poolName}");
                return null;
            }

            poolObjects.RemoveAt(0);
            usedObjects.Add(obj);

            return obj;
        }

        internal void ReturnObject(T obj, string poolName, List<T> poolObjects, List<T> usedObjects)
        {
            if (poolObjects.Contains(obj))
            {
                return;
            }

            if (!usedObjects.Contains(obj))
            {
                Debug.LogError($"The given object {obj} does not belong to this pool {poolName}");
                return;
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