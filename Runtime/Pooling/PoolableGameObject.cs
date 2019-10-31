using UnityEngine;
 
namespace Gameframe.GUI.Pooling
{
    public class PoolableGameObject : MonoBehaviour
    {
        private Pool _pool = null;
        public Pool SourcePool
        {
            get => _pool;
            set => _pool = value;
        }
 
        public virtual void OnDestroy()
        {
            //SourcePool may be null if this is a prefab object
            if ( SourcePool != null )
            {
                SourcePool.RemoveInstance(this);
            }
        }
 
        public virtual void OnPoolableSpawned()
        {
 
        }
 
        public virtual void OnPoolableDespawn()
        {
 
        }
 
        public void Despawn()
        {
            SourcePool.Despawn(this);
        }
 
    }
}