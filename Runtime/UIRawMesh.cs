using UnityEngine;

namespace Gameframe.GUI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UIRawMesh : MonoBehaviour
    {
        private void Start()
        {
            Refresh();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Refresh();
        }
#endif

        private void Refresh()
        {
            var canvasRenderer = this.GetComponent<CanvasRenderer>();
            var meshRenderer = this.GetComponent<MeshRenderer>();
            var meshFilter = this.GetComponent<MeshFilter>();

            if (canvasRenderer == null || meshRenderer == null || meshFilter == null)
            {
                return;
            }

            meshRenderer.enabled = false;
            if (meshFilter.sharedMesh.isReadable)
            {
                canvasRenderer.SetMesh(meshFilter.sharedMesh);
            }
            else
            {
                Debug.LogErrorFormat("Mesh {0} must be marked as readable to be used with UIRawMesh component!",meshFilter.sharedMesh.name);
            }
            canvasRenderer.materialCount = 1;
            canvasRenderer.SetMaterial(meshRenderer.sharedMaterials[0], 0);
        }
    }
}