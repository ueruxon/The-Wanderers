using System;
using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Logic.Buildings
{
    public class Storage : Building
    {
        private const int StorageCapacity = 42;
        
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private Transform _interactionPoint;
        
        [Header("Visual Resource Filler Settings")]
        [Space(4)]
        [SerializeField] private Transform _visualResourceFillerPrefab;
        [SerializeField] private Transform _fillerContainer;

        private Stack<Resource> _storedResources;
        private Transform[] _visualResourceFillers;

        private int _currentResourceIndex;
        private int _reservedPlaceCount;

        private void Awake()
        {
            _storedResources = new Stack<Resource>(StorageCapacity);
            _visualResourceFillers = new Transform[StorageCapacity];
            _currentResourceIndex = 0;
            _reservedPlaceCount = 0;

            GenerateResourceFillers();
        }

        public ResourceType GetStoredResourceType() => 
            _resourceType;

        public bool HasResource() => 
            _storedResources.Count > 0;

        public bool IsFull() => 
            _storedResources.Count == StorageCapacity;

        public bool HasPlaceForReserve() => 
            _reservedPlaceCount + 1 <= StorageCapacity;

        public void ReservePlaceForResource() => 
            _reservedPlaceCount++;

        public void ClearReservedPlace() => 
            _reservedPlaceCount--;

        public void AddResource(Resource resource)
        {
            _visualResourceFillers[_currentResourceIndex].gameObject.SetActive(true);
            _currentResourceIndex++;

            _storedResources.Push(resource);
        }

        public Resource GetResource()
        {
            _visualResourceFillers[_currentResourceIndex].gameObject.SetActive(false);
            _currentResourceIndex--;
            
            ClearReservedPlace();
            
            return _storedResources.Pop();
        }

        public Transform GetInteractionPoint() => 
            _interactionPoint;

        private void GenerateResourceFillers()
        {
            float zStart = -3f;
            float xStart = -1.75f;
            float yStart = 0;

            float xOffset = 3.5f;
            float yOffset = 0.65f;
            
            int width = 2;
            int height = 3;
            int dept = 7;

            int index = 0;

            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    for (int z = 0; z < dept; ++z)
                    {
                        if (x > 0 && x < width - 1 && 
                            y > 0 && y < height - 1 && 
                            z > 0 && z < dept - 1) 
                            continue;

                        //float xOffset = (xStart + (x % width) * xPadding);
                        //float xPosition = (x + xStart) + (x % width) * xPadding;
                        float xPosition = xStart + xOffset * x;
                        float yPosition = yStart + yOffset * y;
                        
                        Vector3 spawnPosition = _fillerContainer.position + new Vector3(
                            xPosition, 
                            yPosition, 
                            z + zStart);
                        
                        Transform filler = Instantiate(_visualResourceFillerPrefab, spawnPosition, Quaternion.identity);
                        filler.SetParent(_fillerContainer);
                        filler.gameObject.SetActive(false);
                        _visualResourceFillers[index] = filler;
                        
                        index++;

                        filler.name = $"Wood Filler: {index}";
                    }
                }
            }
        }
    }
}