using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelType")]
    public class PanelType : ScriptableObject
    {
        public enum Visibility
        {
            Opaque,
            Transparent
        }
        
        //What info should be in the type of a panel?
        //prefab/addressable asset
        //opaque/transparent - shows or hides panels below it in a stack?

        public PanelViewBase prefab;
        public Visibility visibility = Visibility.Opaque;
        
        public virtual Task<PanelViewBase> GetPrefabAsync()
        {
            return Task.FromResult(prefab);
        }
    }
}
