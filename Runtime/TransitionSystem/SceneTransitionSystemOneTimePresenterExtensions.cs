using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Gameframe.GUI.TransitionSystem
{
    public static class SceneTransitionSystemOneTimePresenterExtensions
    {
        public static async void LoadScene(this SceneTransitionSystem transitionSystem, string sceneName, ITransitionPresenter oneTimePresenter, LoadSceneMode mode = LoadSceneMode.Single)
        {
            await LoadSceneAsync(transitionSystem, sceneName, oneTimePresenter, mode);
        }

        public static async Task LoadSceneAsync(this SceneTransitionSystem transitionSystem, string sceneName, ITransitionPresenter oneTimePresenter, LoadSceneMode mode = LoadSceneMode.Single)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadSceneAsync(sceneName,mode);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }
        
        public static async void LoadScenes(this SceneTransitionSystem transitionSystem, string[] loadScenes, string[] unloadScenes, ITransitionPresenter oneTimePresenter)
        {
            await LoadScenesAsync(transitionSystem, loadScenes, unloadScenes, oneTimePresenter);
        }
        
        public static async Task LoadScenesAsync(this SceneTransitionSystem transitionSystem, string[] loadScenes, string[] unloadScenes, ITransitionPresenter oneTimePresenter)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadSceneAsync(loadScenes, unloadScenes);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }

        public static async void LoadScenes(this SceneTransitionSystem transitionSystem, string[] loadScenes, ITransitionPresenter oneTimePresenter)
        {
            await LoadScenesAsync(transitionSystem, loadScenes, oneTimePresenter);
        }
        
        public static async Task LoadScenesAsync(this SceneTransitionSystem transitionSystem, string[] loadScenes, ITransitionPresenter oneTimePresenter)
        {
            transitionSystem.AddPresenter(oneTimePresenter);
            await transitionSystem.LoadScenesAsync(loadScenes);
            transitionSystem.RemovePresenter(oneTimePresenter);
        }
    }
}