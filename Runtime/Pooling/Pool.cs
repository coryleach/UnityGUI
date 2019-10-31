using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.Pooling
{
    public class Pool
    {
        public Pool(PoolableGameObject prefab)
        {
            _prefab = prefab;
        }

        private readonly PoolableGameObject _prefab;
        public PoolableGameObject Prefab => _prefab;

        private readonly List<PoolableGameObject> _instanceQueue = new List<PoolableGameObject>();

        public T Spawn<T>(Transform parent = null ) where T : PoolableGameObject
        {
            Debug.Assert(_prefab is T, "pool prefab is not the correct type");
            return Spawn(parent) as T;
        }

        public PoolableGameObject Spawn(Transform parent = null)
        {
            PoolableGameObject spawnedObject = null;

            if ( _instanceQueue.Count > 0 )
            {
                spawnedObject = _instanceQueue[0];
                _instanceQueue.RemoveAt(0);
            }
            else
            {
                spawnedObject = parent != null ? Object.Instantiate(_prefab,parent) : Object.Instantiate(_prefab);
                spawnedObject.SourcePool = this;
            }

            //If prefab is active, then also activate the spawned game object
            if ( Prefab.gameObject.activeSelf )
            {
                spawnedObject.gameObject.SetActive(true);
            }

            spawnedObject.OnPoolableSpawned();

            return spawnedObject;
        }

        public void Despawn(PoolableGameObject spawnedObject)
        {
            Debug.Assert(spawnedObject.SourcePool == this, "Attempting to despawn an object to a pool that is not its spawn source");

            spawnedObject.OnPoolableDespawn();
            spawnedObject.gameObject.SetActive(false);
            _instanceQueue.Add(spawnedObject);
        }

        public void RemoveInstance(PoolableGameObject instance)
        {
            _instanceQueue.Remove(instance);
        }

    }
}