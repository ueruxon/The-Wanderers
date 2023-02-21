using System;
using Game.Code.Data.StaticData.ResourceNodeData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Code.Logic.ResourcesLogic
{
    public class ResourceNodeSpawner : MonoBehaviour
    {
        public event Action<Resource> ResourceSpawned;

        [SerializeField] private ResourceType _resourceType;
        
        private ResourceNodeData _nodeData;
        private ResourceData _resourceData;
        private ResourceNode _currentNode;

        private bool _isWorkedOut;
        private float _currentTimerToRespawn;
        private float _timeToRespawn;

        public void Init()
        {
            SpawnNode();
        }

        public void Init(ResourceNodeData nodeData, ResourceData resourceData)
        {
            _nodeData = nodeData;
            _resourceData = resourceData;
            _timeToRespawn = _nodeData.TimeToRespawn;
            
            SpawnNode();
        }

        public bool HasResource() => 
            _isWorkedOut == false;

        public ResourceType GetResourceType() => 
            _resourceType;

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
            _currentNode = Instantiate(_nodeData.Prefab, transform);
            _currentNode.NodeDestroyed += OnNodeDestroyed;
            _currentNode.Init(_nodeData);

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
            Resource resource = Instantiate(_resourceData.Prefab, randomPosition, Quaternion.identity);
            resource.name = $"{resource.GetResourceType().ToString()}, Position: {transform.position.x}";
            
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