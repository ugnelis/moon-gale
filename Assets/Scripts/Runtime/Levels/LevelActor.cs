using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace MoonGale.Runtime.Levels
{
    internal sealed class LevelActor : MonoBehaviour
    {
        [Header("General")]
        [SerializeField]
        private LevelSettings levelSettings;

        [SerializeField]
        private bool isGenerateOnAwake = true;

        private readonly IList<ILevelNode> levelNodes = new List<ILevelNode>();

        private void Awake()
        {
            if (isGenerateOnAwake == false)
            {
                return;
            }

            CreateLevelNodes();
            InitializeLevelNodes();
        }

        private void OnDestroy()
        {
            DestroyLevelNodes();
        }

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        [Button("Generate Level")]
        private void GenerateLevelEditor()
        {
            DestroyLevelNodes();
            CreateLevelNodes();
            InitializeLevelNodes();
        }
#endif

#if UNITY_EDITOR
        // ReSharper disable once UnusedMember.Local
        [Button("Destroy Level")]
        private void DestroyLevelEditor()
        {
            DestroyLevelNodes();
        }
#endif

        private void CreateLevelNodes()
        {
            var map = levelSettings.Map;
            var height = map.height;
            var width = map.width;
            var colors = map.GetPixels();

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    var mapIndex = new Vector2Int(x, y);
                    var color = colors[x + y * width];
                    var node = CreateNode(mapIndex, color);

                    levelNodes.Add(node);
                }
            }
        }

        private static ILevelNode CreateNode(Vector2Int mapIndex, Color color)
        {
            if (color == Color.black)
            {
                return new SolidBLockLevelNode(mapIndex, color);
            }

            if (color == Color.green)
            {
                return new SpawnPointLevelNode(mapIndex, color);
            }

            if (color == Color.red)
            {
                return new SourceRootLevelNode(mapIndex, color);
            }

            return new AirLevelNode(mapIndex, color);
        }

        private void InitializeLevelNodes()
        {
            foreach (var levelNode in levelNodes)
            {
                InitializeLevelNode(levelNode);
            }
        }

        private void InitializeLevelNode(ILevelNode levelNode)
        {
            InitializeLevelNodeNeighbors(levelNode);
            InitializeLevelNodeElement(levelNode);
            InitializeLevelNodeElementPosition(levelNode);
        }

        private void InitializeLevelNodeNeighbors(ILevelNode levelNode)
        {
            var neighbors = GetNeighboringNodes(levelNode);
            foreach (var neighbor in neighbors)
            {
                levelNode.AddNeighbor(neighbor);
            }
        }

        private void InitializeLevelNodeElement(ILevelNode levelNode)
        {
            if (levelNode is SolidBLockLevelNode solidBLockLevelNode)
            {
                // TODO: pick prefab variant here!
                var solidBlockElement = CreateSolidBlockElement();
                solidBLockLevelNode.Initialize(solidBlockElement);
            }
        }

        private SolidBlockLevelElement CreateSolidBlockElement()
        {
            return InstantiateElement(levelSettings.SolidBlockPrefab);
        }

        private TElement InstantiateElement<TElement>(TElement elementPrefab) where TElement : LevelElement
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                return (TElement) UnityEditor.PrefabUtility.InstantiatePrefab(elementPrefab, transform);
            }
#endif
            return Instantiate(elementPrefab, transform);
        }

        private void InitializeLevelNodeElementPosition(ILevelNode levelNode)
        {
            if (levelNode.TryGetLevelElement(out var levelElement) == false)
            {
                return;
            }

            var elementTransform = levelElement.transform;
            var elementPosition = elementTransform.position;

            var blockScale = levelSettings.BlockScale;
            var mapIndex = levelNode.MapIndex;

            var map = levelSettings.Map;
            var mapWidth = map.width;
            var mapHeight = map.height;

            // Grid position.
            elementPosition.x = mapIndex.x * blockScale.x - mapWidth * blockScale.x / 2f;
            elementPosition.z = mapIndex.y * blockScale.z - mapHeight * blockScale.z / 2f;

            // Height offset.
            elementPosition.y += blockScale.y / 2f;

            elementTransform.localScale = blockScale;
            elementTransform.position = elementPosition;
        }

        private void DestroyLevelNodes()
        {
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                for (var index = 0; index < transform.childCount; index++)
                {
                    var childTransform = transform.GetChild(index);
                    DestroyImmediate(childTransform.gameObject);
                }
            }
#endif

            foreach (var levelNode in levelNodes)
            {
                DestroyLevelNode(levelNode);
            }

            levelNodes.Clear();
        }

        private static void DestroyLevelNode(ILevelNode levelNode)
        {
            if (levelNode.TryGetLevelElement(out var element))
            {
#if UNITY_EDITOR
                if (Application.isPlaying == false)
                {
                    DestroyImmediate(element.gameObject);
                    return;
                }
#endif

                Destroy(element.gameObject);
            }
        }

        private IReadOnlyList<ILevelNode> GetNeighboringNodes(ILevelNode node, int radius = 1)
        {
            var map = levelSettings.Map;
            var width = map.width;
            var height = map.height;
            var index = node.MapIndex;

            var neighbors = new List<ILevelNode>();
            for (var x = index.x - radius; x <= index.x + radius; x++)
            {
                for (var y = index.y - radius; y <= index.y + radius; y++)
                {
                    if (x < 0 || x >= width || y < 0 || y >= height || x == index.x && y == index.y)
                    {
                        continue;
                    }

                    var neighbor = levelNodes[x + y * width];
                    neighbors.Add(neighbor);
                }
            }

            return neighbors;
        }
    }
}
