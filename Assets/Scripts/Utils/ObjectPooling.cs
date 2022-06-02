using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BSTW.Utils
{
    public class ObjectPooling : MonoBehaviour
    {
        [SerializeField] private GameObject _objectPrefab;
        [SerializeField] private int _amount = 5;

        private List<GameObject> _pooledObjects = new List<GameObject>();

        private void Awake()
        {
            InstantiateObjects();
        }

        private void InstantiateObjects()
        {
            for (int i = 0; i < _amount; i++)
            {
                GameObject pooledObject = Instantiate(_objectPrefab);
                pooledObject.SetActive(false);
                _pooledObjects.Add(pooledObject);
            }
        }

        public GameObject GetObject()
        {
            GameObject pooledObject = _pooledObjects.FirstOrDefault(o => !o.activeSelf);
            if (pooledObject == null)
            {
                pooledObject = Instantiate(_objectPrefab);
                _pooledObjects.Add(pooledObject);
            }

            pooledObject.SetActive(true);
            return pooledObject;
        }
    }
}

