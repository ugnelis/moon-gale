using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal interface ILevelNode<TLevelElement> where TLevelElement : LevelElement
    {
        public void Initialize(TLevelElement newLevelElement);

        public bool TryGetLevelElement(out TLevelElement element);
    }

    internal interface ILevelNode
    {
        public Vector2Int MapIndex { get; }

        public Color Color { get; }

        public IReadOnlyList<ILevelNode> Neighbors { get; }

        public void RemoveNeighbor(ILevelNode node);

        public void AddNeighbor(ILevelNode node);

        public void ClearNeighbors();

        public bool TryGetLevelElement(out LevelElement element);
    }
}
