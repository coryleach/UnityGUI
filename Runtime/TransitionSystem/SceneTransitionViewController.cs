using System;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.PanelSystem;
using UnityEngine.Events;

namespace Gameframe.GUI.TransitionSystem
{
    public class SceneTransitionViewController : PanelViewControllerBehaviour, ITransitionPresenter
    {
        public UIEventManager eventManager;
        public SceneTransitionSystem sceneTransitionSystem;

        [Serializable]
        public class ProgressEvent : UnityEvent<float> {}

        public ProgressEvent progressEvent = new ProgressEvent();

        private void OnEnable()
        {
            sceneTransitionSystem.AddPresenter(this);
        }

        private void OnDisable()
        {
            sceneTransitionSystem.RemovePresenter(this);
        }

        public async Task StartTransitionAsync()
        {
            if (!IsViewLoaded)
            {
                await LoadViewAsync();
            }
            eventManager.Lock();
            await ShowAsync();
            eventManager.Unlock();
        }

        public virtual void TransitionProgress(float progress)
        {
            progressEvent.Invoke(progress);
        }

        public async Task FinishTransitionAsync()
        {
            eventManager.Lock();
            await HideAsync();
            eventManager.Unlock();
        }
    }
}
