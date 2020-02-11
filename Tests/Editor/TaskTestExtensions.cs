using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Gameframe.GUI.PanelSystem.Tests.Editor
{
    public static class TaskTestExtensions
    {
        public static IEnumerator AsCoroutine(this Task task)
        {
            while (!task.IsCompleted)
            {
                yield return null;
            }
            
            if (task.Status == TaskStatus.Faulted)
            {
                Debug.LogException(task.Exception);
            }
        }
    }
}