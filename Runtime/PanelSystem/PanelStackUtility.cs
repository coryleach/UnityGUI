using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public static class PanelStackUtility
    {
        public static void GetVisiblePanelViewControllers(IPanelStackSystem stack, IList<IPanelViewController> output)
        {
            for (int i = stack.Count-1; i >= 0; i--)
            {
                var currentController = stack[i];
                
                output.Add(currentController);
                
                //Stop as soon as we hit a panel that is vully opaque
                if (currentController.PanelType.visibility == PanelType.Visibility.Opaque)
                {
                    return;
                }
            }
        }

        public static void GetControllersForOptions(IEnumerable<IPanelViewController> controllers, IEnumerable<IPanelViewController> optionsList, List<IPanelViewController> output)
        {
            output.Clear();
            
            var selectedControllers = optionsList.Select(x =>
            {
                return controllers.First(y => y.PanelType == x.PanelType);
            });
            
            output.AddRange(selectedControllers);
        }
    }
}