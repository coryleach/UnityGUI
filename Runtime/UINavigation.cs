using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Gameframe.GUI
{
    /// <summary>
    /// Contains components that help with keyboard/controller navigation of UI
    /// Requires a component of type UnityEngine.UI.Selectable
    /// </summary>
    public class UINavigation : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        [SerializeField] private Selectable targetSelectable;

        [SerializeField] private SelectByDefaultType selectedByDefault;

        [SerializeField] private bool onlySelectWhenInteractable = true;

        [SerializeField] private UnityEvent OnSelected = new UnityEvent();

        [SerializeField] private UnityEvent OnDeselected = new UnityEvent();


        private bool CanSelect =>
            targetSelectable != null && (targetSelectable.interactable || !onlySelectWhenInteractable);

        public enum SelectByDefaultType
        {
            NotSelected,
            OnStart,
            OnEnable
        }

        private void Start()
        {
            if (selectedByDefault == SelectByDefaultType.OnStart && targetSelectable != null)
            {
                targetSelectable.Select();
            }
        }

        private void OnEnable()
        {
            if (selectedByDefault == SelectByDefaultType.OnEnable && targetSelectable != null)
            {
                targetSelectable.Select();
            }
        }

        private void OnValidate()
        {
            if (targetSelectable == null)
            {
                targetSelectable = GetComponent<Selectable>();
            }
        }

        public async void OnSelect(BaseEventData eventData)
        {
            if (targetSelectable == null)
            {
                return;
            }

            if (CanSelect)
            {
                OnSelected.Invoke();
                return;
            }

            if (!(eventData is AxisEventData axisEvent))
            {
                return;
            }

            var next = targetSelectable.FindSelectable(axisEvent.moveVector);

            while (next != null && !next.interactable && next != targetSelectable)
            {
                next = next.FindSelectable(axisEvent.moveVector);
            }

            if (next != null && next != targetSelectable)
            {
                await Task.Yield();
                next.Select();
            }
        }

        public void OnDeselect(BaseEventData eventData)
        {
            OnDeselected.Invoke();
        }
    }
}
