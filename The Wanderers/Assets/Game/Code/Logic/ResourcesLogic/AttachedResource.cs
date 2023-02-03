using UnityEngine;

namespace Game.Code.Logic.ResourcesLogic
{
    public class AttachedResource : MonoBehaviour
    {
        [SerializeField] private Transform _attachTransform;

        private Transform _unit;
        private Resource _currentResource;

        public void Init(Transform unitTransform)
        {
            _unit = unitTransform;
        }

        public void Attach(Resource resource)
        {
            _currentResource = resource;
            _currentResource.transform.SetParent(_attachTransform);
            _currentResource.transform.position = _attachTransform.position;
            _currentResource.transform.rotation = _attachTransform.rotation;
        }

        public void Detach()
        {
            Destroy(_currentResource.gameObject);
            _currentResource = null;
        }

        private void LateUpdate()
        {
            _attachTransform.rotation = Quaternion.Slerp(_attachTransform.rotation, _unit.rotation, 5f * Time.deltaTime);
        }
    }
}