using UnityEngine;

namespace Padoru.ObjectPooling
{
    [CreateAssetMenu(menuName = "Padoru/Pools/GameObjectPool")]
    public class GameObjectPool : ScriptableObjectPool<GameObject>
    {
        private static Transform poolParent;

        [SerializeField] private GameObject prefab;

        protected override void Awake()
        {
            base.Awake();

            if (!Application.isPlaying)
            {
                return;
            }

            if(poolParent == null)
            {
                var go = new GameObject("PoolsParent");
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

        protected override void OnGetObject(GameObject obj)
        {
            obj.transform.SetParent(null);
            obj.SetActive(true);
        }

        protected override void OnReturnObject(GameObject obj)
        {
            obj.transform.SetParent(poolParent);
            obj.SetActive(false);
        }
    }
}