using Game.Code.Core;
using UnityEngine;

namespace Game.Code.Logic.ResourcesLogic
{
    public class ResourceNodeSpawner : MonoBehaviour
    {
        [SerializeField] private ResourceType _resourceType;
        [SerializeField] private ResourceNode _resourceNodePrefab;
        [SerializeField] private Resource _resourcePrefab;

        private DynamicGameContext _dynamicGameContext;
        private ResourceNode _currentNode;
        
        private bool _isWorkedOut;
        private float _currentTimerToRespawn;
        private float _timeToRespawn = 20f;

        public void Init(DynamicGameContext dynamicGameContext)
        {
            _dynamicGameContext = dynamicGameContext;

            SpawnNode();
        }
        
        public bool HasResource() => 
            _isWorkedOut == false;

        // может быть null
        public ResourceNode GetResourceNode() => 
            _currentNode;
        
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
            _currentNode = Instantiate(_resourceNodePrefab, transform);
            _currentNode.NodeDestroyed += OnNodeDestroyed;
            _currentNode.Init();

            _isWorkedOut = false;
            _currentTimerToRespawn = 0f;
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
            Resource resource = Instantiate(_resourcePrefab, randomPosition, Quaternion.identity);
            resource.name = $"Wood: {transform.position.x}";
            
            _dynamicGameContext.AddResource(resource);
        }
        
        private Vector3 GetRandomPositionOnRadius(Vector3 startPosition)
        {
            float randomRadius = Random.Range(1, 3);

            float randomAngle = Random.Range(0f, 360f);
            float x = randomRadius * Mathf.Cos(randomAngle);
            float z = randomRadius * Mathf.Sin(randomAngle);
            
            return new Vector3(startPosition.x + x, .1f, startPosition.z + z);
        }
        
        private void OnDrawGizmos()
        {
            if (_resourceType == ResourceType.Wood)
                Gizmos.color = Color.yellow;
            if (_resourceType == ResourceType.Stone)
                Gizmos.color = Color.gray;
            if (_resourceType == ResourceType.Ore)
                Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}