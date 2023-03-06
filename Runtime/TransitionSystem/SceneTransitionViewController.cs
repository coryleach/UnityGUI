using System;
using System.Threading.Tasks;
using Gameframe.GUI.Camera.UI;
using Gameframe.GUI.PanelSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Gameframe.GUI.TransitionSystem
{
    public class SceneTransitionViewController : PanelViewControllerBehaviour, ITransitionPresenter
    {
        [SerializeField]
        private UIEventManager eventManager;

        [SerializeField]
        private SceneTransitionSystem sceneTransitionSystem;

        [Serializable]
        public class ProgressEvent : UnityEvent<float> {}

        private readonly ProgressEvent onProgressUpdate = new ProgressEvent();
        public ProgressEvent OnProgressUpdate => onProgressUpdate;

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

        public virtual Task PreTransitionAsync()
        {
            return Task.FromResult(true);
        }

        public virtual void TransitionProgress(float progress)
        {
            onProgressUpdate.Invoke(progress);
        }

        public virtual Task PostTransitionAsync()
        {
            return Task.FromResult(true);
        }

        public async Task FinishTransitionAsync()
        {
            eventManager.Lock();
            await HideAsync();
            eventManager.Unlock();
        }
    }
}
