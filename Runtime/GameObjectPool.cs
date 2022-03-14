using UnityEngine;

namespace Padoru.ObjectPooling
{
    [CreateAssetMenu(menuName = "Padoru/Pools/GameObjectPool")]
    public class GameObjectPool : ScriptableObjectPool<GameObject>
    {
        private static Transform poolParent;

        [SerializeField] private GameObject prefab;

        protected override void OnInitialization()
        {
            if(poolParent == null)
            {
                var go = new GameObject("PoolParent");
                poolParent = go.transform;
            }
        }

        protected override GameObject CreateObject()
        {
            var go = Instantiate(prefab);
            go.transform.SetParent(poolParent);
            go.SetActive(false);
            return go;
        }

        protected override void OnGetObject(GameObject go)
        {
            go.transform.SetParent(null);
            go.SetActive(true);
        }

        protected override void OnReturnObject(GameObject go)
        {
            go.transform.SetParent(poolParent);
            go.SetActive(false);
        }
    }
}