using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Code.Logic.Selection
{
    public class SelectionHandler : MonoBehaviour
    {
        [SerializeField] private Transform _selectedArea;

        [SerializeField] private LayerMask _selectionObjectsMask;
        [SerializeField] private LayerMask _mouseColliderMask;
        
        private UnityEngine.Camera _camera;
        private PlayerInput _playerInput;

        // для теста
        [SerializeField] private SelectionMode _currentMode = SelectionMode.Tree;

        private Vector3 _startDragPosition;
        private Vector3 _selectionCenterPosition;

        private bool _selection;

        private Dictionary<ResourceType, List<ResourceNode>> _resourceNodesByType;

        public void Init()
        {
            _camera = UnityEngine.Camera.main;
            _selection = false;

            _resourceNodesByType = new Dictionary<ResourceType, List<ResourceNode>>()
            {
                [ResourceType.Wood] = new (),
                [ResourceType.Stone] = new ()
            };

            // вынести в отдельный инпут
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            _playerInput.Selection.Select.performed += OnMousePressed;
            _playerInput.Selection.Select.canceled += OnMouseRaised;
            

            // делать инстаншиейт и загружать префаб через статик дату
            _selectedArea.gameObject.SetActive(false);
        }

        private void Update()
        {
            if (_currentMode == SelectionMode.None)
                return;

            if (_selection) 
                DrawSelectedArea();
        }

        private void OnMousePressed(InputAction.CallbackContext inputValue)
        {
            Vector2 pointerPosition = _playerInput.Selection.PointerPosition.ReadValue<Vector2>();
            _startDragPosition = GetMouseWorldPosition(pointerPosition);

            _selection = true;
            _selectedArea.gameObject.SetActive(true);
        }

        private void OnMouseRaised(InputAction.CallbackContext inputValue)
        {
            SelectObjects();

            foreach (ResourceNode resourceNode in _resourceNodesByType[ResourceType.Wood])
            {
                print(resourceNode.transform.position);
            }
            
            _selection = false;

            _selectedArea.localScale = Vector3.zero;
            _selectedArea.gameObject.SetActive(false);
        }

        private void SelectObjects()
        {
            Vector3 centerArea = new Vector3(_selectedArea.localScale.x * .5f, 5f, _selectedArea.localScale.z * .5f);

            bool isSingleTarget = false;

            if (centerArea.x < .5f)
            {
                centerArea.x = .35f;
                isSingleTarget = true;
            }

            if (centerArea.z < .5f)
            {
                centerArea.z = .35f;
                isSingleTarget = true;
            }

            Collider[] hitColliders = Physics.OverlapBox(_selectionCenterPosition, centerArea, Quaternion.identity,
                _selectionObjectsMask);

            foreach (Collider collider in hitColliders)
            {
                if (collider.TryGetComponent(out ResourceNode node)) 
                    AddSelectResourceNode(node);

                if (isSingleTarget)
                    break;
            }
        }

        private void AddSelectResourceNode(ResourceNode node)
        {
            ResourceType nodeType = node.GetNodeType();

            if (nodeType == ResourceType.Wood && _currentMode == SelectionMode.Tree)
            {
                if (_resourceNodesByType[nodeType].Contains(node) == false)
                    _resourceNodesByType[nodeType].Add(node);
            }
            
            if (nodeType == ResourceType.Stone && _currentMode == SelectionMode.Stone)
            {
                if (_resourceNodesByType[nodeType].Contains(node) == false)
                    _resourceNodesByType[nodeType].Add(node);
            }
        }

        private void DrawSelectedArea()
        {
            Vector2 pointerPosition = _playerInput.Selection.PointerPosition.ReadValue<Vector2>();
            Vector3 currentPosition = GetMouseWorldPosition(pointerPosition);


            // нижний левый угол (начало)
            Vector3 lowerLeft = new Vector3(
                Mathf.Min(_startDragPosition.x, currentPosition.x),
                0.1f,
                Mathf.Min(_startDragPosition.z, currentPosition.z)
            );
            // правый верхний
            Vector3 upperRight = new Vector3(
                Mathf.Max(_startDragPosition.x, currentPosition.x),
                0.1f,
                Mathf.Max(_startDragPosition.z, currentPosition.z)
            );

            // центр зоны выделения
            _selectionCenterPosition = new Vector3(
                lowerLeft.x + ((upperRight.x - lowerLeft.x) * .5f),
                0,
                lowerLeft.z + ((upperRight.z - lowerLeft.z) * .5f)
            );

            _selectedArea.position = lowerLeft;
            _selectedArea.localScale = upperRight - lowerLeft;
        }

        private Vector3 GetMouseWorldPosition(Vector2 screenPosition)
        {
            Ray ray = _camera.ScreenPointToRay(screenPosition);

            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _mouseColliderMask))
                return raycastHit.point;

            return Vector3.zero;
        }

        private void OnDrawGizmos()
        {
            Vector3 area = new Vector3(_selectedArea.localScale.x, 5f, _selectedArea.localScale.z);

            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_selectionCenterPosition + transform.position, area);
        }
    }
}