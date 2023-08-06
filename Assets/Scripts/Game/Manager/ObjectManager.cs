using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manager
{
    public class ObjectManager : SingletonMonoBehaviour<ObjectManager>
    {
        private List<GameObject> _gameObjectList;

        private void Awake()
        {
            _gameObjectList = new List<GameObject>();
            _gameObjectList.Clear();
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void AddList(GameObject obj)
        {
            _gameObjectList.Add(obj);
        }
    }
}