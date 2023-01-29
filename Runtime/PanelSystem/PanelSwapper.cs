using UnityEngine;
using UnityEngine.UI;

namespace Gameframe.GUI.PanelSystem
{
   public class PanelSwapper : MonoBehaviour
   {
     [SerializeField] private ScriptablePanelSwapSystem swapSystem;

     [SerializeField, Help("If controller is null PanelSwapper will attempt to get the controller from the provider using PanelType")] 
     private PanelViewControllerBehaviour controller;

     [SerializeField] private PanelType panelType;

     [SerializeField] private PanelViewControllerProvider controllerProvider;
     
     [SerializeField] private SwapEvent swapEvent = SwapEvent.Manual;

     [Header("Button"), SerializeField] private Button button;
     [SerializeField] private bool controlButtonInteractivity;

     [Header("Image Colors"), SerializeField] private Image image;
     [SerializeField] private bool controlImageTint;
     [SerializeField] private Color activeColor = Color.white;
     [SerializeField] private Color disabledColor = Color.grey;
     
     public enum SwapEvent
     {
       Manual,
       ButtonClick,
       Awake,
       Start,
       Enable
     }
     
     private void Awake()
     {
       if (swapEvent == SwapEvent.Awake)
       {
         Swap();
       }
       else if (swapEvent == SwapEvent.ButtonClick)
       {
         button?.onClick.AddListener(Swap);
       }
     }
     
     private void Start()
     {
       if (swapEvent == SwapEvent.Start)
       {
         Swap();
       }
     }
     
     private void OnEnable()
     {
       if (swapSystem != null)
       {
         swapSystem.OnSwap.AddListener(OnSwapped);
       }
       if (swapEvent == SwapEvent.Enable)
       {
         Swap();
       }
     }

     private void OnDisable()
     {
       if (swapSystem != null)
       {
         swapSystem.OnSwap.RemoveListener(OnSwapped);
       }
     }

     private void OnSwapped()
     {
       Refresh();
     }

     private void Refresh()
     {
       if (swapSystem == null)
       {
         return;
       }
       
       //If the current view contr
       var currentViewController = swapSystem.CurrentViewController;
       var showingOurPanel = false;

       if (controller != null)
       {
         showingOurPanel = ReferenceEquals(controller, currentViewController);
       }
       else
       {
         showingOurPanel = currentViewController.PanelType == panelType;
       }
       
       if (controlButtonInteractivity && button != null)
       {
         if (showingOurPanel)
         {
           //Current showing panel is the panel for this button so disable this button
           button.interactable = false;
         }
         else
         {
           //Currently showing a panel different than the panel this button will show so let's enable the button
           button.interactable = true;
         }
       }
       if (controlImageTint && image != null)
       {
         image.color = showingOurPanel ? disabledColor : activeColor;
       }
     }
     
     public async void Swap()
     {
       IPanelViewController swapController = controller;
       if (swapController == null)
       {
         swapController = controllerProvider.Get(panelType);
       }
       await swapSystem.ShowAsync(swapController);
       Refresh();
     }

     private void OnValidate()
     {
       if (button == null)
       {
         button = GetComponent<Button>();
       }
       if (image == null)
       {
         image = GetComponent<Image>();
       }
     }
     
   } 
}


