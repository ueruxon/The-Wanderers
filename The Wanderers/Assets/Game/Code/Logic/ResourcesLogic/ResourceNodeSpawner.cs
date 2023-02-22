using System;
using Game.Code.Data.StaticData.ResourceNodeData;
using Game.Code.Infrastructure.Factories;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Code.Logic.ResourcesLogic
{
    public class ResourceNodeSpawner : MonoBehaviour
    {
        public event Action<Resource> ResourceSpawned;

        private ResourceType _resourceType;
        
        private IResourceMiningFactory _gameFactory;
        private ResourceNodeData _nodeData;
        private ResourceNode _currentNode;

        private bool _isWorkedOut;
        private float _currentTimerToRespawn;
        private float _timeToRespawn;
        
        public void Init(ResourceType resourceType, ResourceNodeData nodeData, IResourceMiningFactory gameFactory)
        {
            _resourceType = resourceType;
            _nodeData = nodeData;
            _gameFactory = gameFactory;
            _timeToRespawn = _nodeData.TimeToRespawn;
            
            SpawnNode();
        }
        
        private void Update()
        {
            if (_isWorkedOut)
            {
                _currentTimerToRespawn += Time.deltaTime;
                
                if (_currentTimerToRespawn >= _timeToRespawn)
                    SpawnNode();
            }
        }

        private void SpawnNode()
        {
            _currentNode = _gameFactory.CreateResourceNode(_resourceType, at: transform);
            _currentNode.NodeDestroyed += OnNodeDestroyed;

            _isWorkedOut = false;
            _currentTimerToRespawn = 0f;
            _timeToRespawn = _nodeData.TimeToRespawn;
        }

        private void OnNodeDestroyed()
        {
            _isWorkedOut = true;

            _currentNode.NodeDestroyed -= OnNodeDestroyed;
            _currentNode = null;
            
            SpawnResource();
        }

        private void SpawnResource()
        {
            Vector3 randomPosition = GetRandomPositionOnRadius(transform.position);
            Resource resource =_gameFactory.CreateResource(_resourceType, randomPosition);
            
            ResourceSpawned?.Invoke(resource);
        }

        private Vector3 GetRandomPositionOnRadius(Vector3 startPosition)
        {
            float randomRadius = Random.Range(2, 4);

            float randomAngle = Random.Range(0f, 360f);
            float x = randomRadius * Mathf.Cos(randomAngle);
            float z = randomRadius * Mathf.Sin(randomAngle);
            
            return new Vector3(startPosition.x + x, .2f, startPosition.z + z);
        }

        private void OnDrawGizmos()
        {
            if (_resourceType == ResourceType.Wood)
                Gizmos.color = Color.yellow;
            if (_resourceType == ResourceType.Stone)
                Gizmos.color = Color.gray;
            if (_resourceType == ResourceType.Coal)
                Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.position, .5f);
        }
    }
}