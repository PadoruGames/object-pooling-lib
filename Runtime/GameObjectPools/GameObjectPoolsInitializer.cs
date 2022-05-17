using Padoru.Core;
using System.Collections.Generic;
using UnityEngine;

namespace Padoru.ObjectPooling
{
    public class GameObjectPoolsInitializer : MonoBehaviour, IInitializable
    {
        [SerializeField] private List<GameObjectPool> pools;

        public void Init()
        {
            foreach (var pool in pools)
            {
                pool.Init();
            }
        }
    }
}