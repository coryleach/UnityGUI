using System.Collections.Generic;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
   
    [CreateAssetMenu(menuName = "Gameframe/PanelSystem/PanelViewControllerProvider")]
    public class PanelViewControllerProvider : ScriptableObject, IPanelViewControllerProvider
    {
        private readonly Dictionary<PanelType,IPanelViewController> controllers = new Dictionary<PanelType, IPanelViewController>();
        
        private void OnEnable()
        {
            controllers.Clear();    
        }

        public void Add(IPanelViewController controller)
        { 
            controllers.Add(controller.PanelType,controller);   
        }

        public void Remove(IPanelViewController controller)
        {
            controllers.Remove(controller.PanelType);
        }

        public IPanelViewController GetOrCreate(PanelType type)
        {
            if (controllers.TryGetValue(type, out var controller))
            {
                return controller;
            }
            controller = new PanelViewController(type);
            controllers.Add(type,controller);
            return controller;
        }
        
        public T Get<T>(PanelType type) where T : class, IPanelViewController
        {
            return Get(type) as T;
        }

        public IPanelViewController Get(PanelType type)
        {
            return controllers.TryGetValue(type, out var controller) ? controller : default;
        }

        public void Clear()
        {
            controllers.Clear();
        }
    } 
}

