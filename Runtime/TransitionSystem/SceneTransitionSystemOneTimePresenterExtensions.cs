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
        
        public static async void LoadScenes(this SceneTransitionSystem transitionSystem, string[] loadScenes, string[] unloadScenes, ITransitionPresenter oneTimePresenter)
        {
            await LoadScenesAsync(transitionSystem, loadScenes, unloadScenes, oneTimePresenter).ConfigureAwait(false);
        }
        
        public static async Task LoadScenesAsync(this SceneTransitionSystem transitionSystem, string[] loadScenes, string[] unloadScenes, ITransitionPresenter oneTimePresenter)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadScenesAsync(loadScenes, unloadScenes).ConfigureAwait(true);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }

        public static async void LoadScenes(this SceneTransitionSystem transitionSystem, string[] loadScenes, ITransitionPresenter oneTimePresenter)
        {
            await LoadScenesAsync(transitionSystem, loadScenes, oneTimePresenter).ConfigureAwait(false);
        }
        
        public static async Task LoadScenesAsync(this SceneTransitionSystem transitionSystem, string[] loadScenes, ITransitionPresenter oneTimePresenter)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadScenesAsync(loadScenes).ConfigureAwait(true);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }
    }
}