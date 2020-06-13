using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Gameframe.GUI.TransitionSystem
{
    public static class SceneTransitionSystemOneTimePresenterExtensions
    {
        public static async void LoadScene(this SceneTransitionSystem transitionSystem, string sceneName, ITransitionPresenter oneTimePresenter, LoadSceneMode mode = LoadSceneMode.Single)
        {
            await LoadSceneAsync(transitionSystem, sceneName, oneTimePresenter, mode).ConfigureAwait(false);
        }

        public static async Task LoadSceneAsync(this SceneTransitionSystem transitionSystem, string sceneName, ITransitionPresenter oneTimePresenter, LoadSceneMode mode = LoadSceneMode.Single)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadSceneAsync(sceneName,mode).ConfigureAwait(true);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }
        
        public static async void LoadScenes(this SceneTransitionSystem transitionSystem, string[] scenesToLoad, string[] scenesToUnload, ITransitionPresenter oneTimePresenter)
        {
            await LoadScenesAsync(transitionSystem, scenesToLoad, scenesToUnload, oneTimePresenter).ConfigureAwait(false);
        }
        
        public static async Task LoadScenesAsync(this SceneTransitionSystem transitionSystem, string[] scenesToLoad, string[] scenesToUnload, ITransitionPresenter oneTimePresenter)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadScenesAsync(scenesToLoad, scenesToUnload).ConfigureAwait(true);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }

        public static async void LoadScenes(this SceneTransitionSystem transitionSystem, string[] scenesToLoad, ITransitionPresenter oneTimePresenter)
        {
            await LoadScenesAsync(transitionSystem, scenesToLoad, oneTimePresenter).ConfigureAwait(false);
        }
        
        public static async Task LoadScenesAsync(this SceneTransitionSystem transitionSystem, string[] scenesToLoad, ITransitionPresenter oneTimePresenter)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadScenesAsync(scenesToLoad).ConfigureAwait(true);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }
    }
}