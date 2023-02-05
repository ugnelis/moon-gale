using DG.Tweening;
using UnityEngine;

namespace MoonGale.Runtime.Levels.Nodes
{
    internal sealed class RootNodeObject : NodeObject
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

        private void Start()
        {
            transform.localScale = Vector3.zero;
            GrowRoot();
        }

        private void GrowRoot()
        {
            var targetScale = levelSettings.RootScale;
            transform
                .DOScale(targetScale, levelSettings.RootScaleDurationSeconds)
                .SetEase(levelSettings.RootScaleEaseAnimation);
        }
    }
}
