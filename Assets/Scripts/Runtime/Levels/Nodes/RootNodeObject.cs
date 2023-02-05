using DG.Tweening;
using UnityEngine;

namespace MoonGale.Runtime.Levels.Nodes
{
    internal sealed class RootNodeObject : NodeObject
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

        [SerializeField]
        private Transform bodyTransform;

        [SerializeField]
        private Transform vfxTransform;

        private void Start()
        {
            bodyTransform.localScale = Vector3.zero;
            GrowRoot();
        }

        private void GrowRoot()
        {
            var targetScale = levelSettings.RootScale;

            if (vfxTransform)
            {
                var vfxScale = vfxTransform.localScale;
                vfxScale.x = targetScale;
                vfxScale.z = targetScale;
                vfxTransform.localScale = vfxScale;
            }

            bodyTransform
                .DOScale(targetScale, levelSettings.RootScaleDurationSeconds)
                .SetEase(levelSettings.RootScaleEaseAnimation);
        }
    }
}
