using System.Collections.Generic;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal abstract class LevelNode : ILevelNode
    {
        public Vector2Int MapIndex { get; }

        public Color Color { get; }

        public IReadOnlyList<ILevelNode> Neighbors => neighbors;

        private readonly List<ILevelNode> neighbors = new();
        private LevelElement levelElement;

        protected LevelNode(Vector2Int mapPosition, Color color)
        {
            MapIndex = mapPosition;
            Color = color;
        }

        public void Initialize(LevelElement newLevelElement)
        {
            levelElement = newLevelElement;
        }

        public bool TryGetLevelElement(out LevelElement element)
        {
            if (levelElement == false)
            {
                element = default;
                return false;
            }

            element = levelElement;
            return true;
        }

        public void RemoveNeighbor(ILevelNode node)
        {
            neighbors.Remove(node);
        }

        public void AddNeighbor(ILevelNode node)
        {
            neighbors.Add(node);
        }

        public void ClearNeighbors()
        {
            neighbors.Clear();
        }
    }
}
