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
        private Transform vfxTransform;

        private void Start()
        {
            transform.localScale = Vector3.zero;
            GrowRoot();
        }

        private void GrowRoot()
        {
            var targetScale = levelSettings.RootScale;

            if (vfxTransform)
            {
                var vfxPosition = vfxTransform.position;
                vfxPosition.y = targetScale / 2f;
                vfxTransform.position = vfxPosition;
            }

            transform
                .DOScale(targetScale, levelSettings.RootScaleDurationSeconds)
                .SetEase(levelSettings.RootScaleEaseAnimation);
        }
    }
}
