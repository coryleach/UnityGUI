using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem
{
    public static class PanelStackUtility
    {
        public static void GetVisiblePanels(IPanelStackSystem stack, IList<PushPanelOptions> output)
        {
            for (int i = stack.Count-1; i >= 0; i--)
            {
                var currentOptions = stack[i];
                
                output.Add(currentOptions);
                
                //Stop as soon as we hit a panel that is vully opaque
                if (currentOptions.panelType.visibility == PanelType.Visibility.Opaque)
                {
                    return;
                }
            }
        }

        public static void GetControllersForOptions(IEnumerable<PanelViewController> controllers, IEnumerable<PushPanelOptions> optionsList, List<PanelViewController> output)
        {
            output.Clear();
            
            var selectedControllers = optionsList.Select(x =>
            {
                return controllers.First(y => y.PanelType == x.panelType);
            });
            
            output.AddRange(selectedControllers);
        }
    }
}