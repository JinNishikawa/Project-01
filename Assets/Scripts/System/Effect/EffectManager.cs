using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class EffectManager : SingletonMonoBehaviour<EffectManager>
    {
        private List<GameObject> _effectList;

        private void Awake()
        {
            _effectList = new List<GameObject>();
            Clear();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public GameObject StartEffect(GameObject soueceObject, Vector3 pos)
        {
            GameObject effectObject = Instantiate(soueceObject, pos, Quaternion.identity);
            effectObject.AddComponent<Effect.EffectBase>();

            return effectObject;
        }

        public GameObject StartEffect(GameObject soueceObject, Vector3 pos, float scaleRate)
        {
            GameObject effectObject = Instantiate(soueceObject, pos, Quaternion.identity);
            effectObject.AddComponent<Effect.EffectBase>();
            effectObject.transform.localScale = scaleRate * Vector3.one;

            return effectObject;
        }

        public GameObject StartEffect(GameObject soueceObject, Transform parent)
        {
            GameObject effectObject = Instantiate(soueceObject, parent);
            effectObject.AddComponent<Effect.EffectBase>();

            return effectObject;
        }

        public void DeleteList(GameObject effectObject)
        {
            _effectList.Remove(effectObject);
            Destroy(effectObject);
        }

        public void Clear()
        {
            foreach(GameObject obj in _effectList)
            {
                Destroy(obj);
            }
            _effectList.Clear();
        }
    }

}