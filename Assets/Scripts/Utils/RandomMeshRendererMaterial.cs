using UnityEngine;
using System.Linq;

namespace BSTW.Utils
{
    public class RandomMeshRendererMaterial : MonoBehaviour
    {
        [SerializeField] private Material[] _materials;
        [SerializeField] private MeshRenderer[] _renderers;
        [SerializeField] private SkinnedMeshRenderer[] _skinnedMeshRenderer;

        private void OnEnable()
       {
            var randomMaterial = _materials[Random.Range(0, _materials.Length - 1)];

            _renderers.ToList().ForEach(r => r.material = randomMaterial);
            _skinnedMeshRenderer.ToList().ForEach(r => r.material = randomMaterial);
        }
    }
}