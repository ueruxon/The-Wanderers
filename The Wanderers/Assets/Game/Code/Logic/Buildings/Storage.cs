using System;
using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;

namespace Game.Code.Logic.Buildings
{
    public class Storage : Building
    {
        private const int StorageCapacity = 42;
        
        [Header("Visual Resource Filler Settings")]
        [Space(4)]
        [SerializeField] private Transform _visualResourceFillerPrefab;
        [SerializeField] private Transform _fillerContainer;

        private ResourceType _resourceType;

        private Stack<Resource> _storedResources;
        private Transform[] _visualResourceFillers;

        private int _currentResourceIndex;

        private void Awake()
        {
            _storedResources = new Stack<Resource>(StorageCapacity);
            _visualResourceFillers = new Transform[StorageCapacity];
            _currentResourceIndex = 0;
            
            GenerateResourceFillers();
            
            print(_visualResourceFillers[_currentResourceIndex]);
        }

        public bool HasResource() => 
            _storedResources.Count > 0;

        public bool IsFull() => 
            _storedResources.Count == StorageCapacity;

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
            
            return _storedResources.Pop();
        }

        private void GenerateResourceFillers()
        {
            float zStart = -3f;
            float xStart = -1.75f;
            float yStart = 0;

            float xOffset = 3.5f;
            float yOffset = 0.75f;
            
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