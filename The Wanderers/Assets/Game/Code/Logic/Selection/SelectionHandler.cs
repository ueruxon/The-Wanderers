using System;
using System.Collections.Generic;
using Game.Code.Logic.ResourcesLogic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Code.Logic.Selection
{
    public class SelectionHandler : MonoBehaviour
    {
        public event Action<SelectionMode> ResourceNodeSelected;
        public event Action<SelectionMode> SelectionModeChanged; 

        [SerializeField] private Transform _selectedArea;
        [SerializeField] private LayerMask _selectionObjectsMask;
        [SerializeField] private LayerMask _mouseColliderMask;

        private UnityEngine.Camera _camera;
        private PlayerInput _playerInput;
        
        private List<ResourceNode> _selectedResourceNodes;

        private Vector3 _startDragPosition;
        private Vector3 _selectionCenterPosition;

        private SelectionMode _currentMode;
        private bool _selection;
        
        public void Init()
        {
            _camera = UnityEngine.Camera.main;
            _selection = false;
            _currentMode = SelectionMode.None;
            _selectedResourceNodes = new List<ResourceNode>();

            // вынести в отдельный инпут
            _playerInput = new PlayerInput();
            _playerInput.Enable();
            _playerInput.Selection.Select.performed += OnMousePressed;
            _playerInput.Selection.Select.canceled += OnMouseRaised;
            _playerInput.Selection.Cancel.performed += OnSelectCanceled;
            
            // делать инстаншиейт и загружать префаб через статик дату
            _selectedArea.gameObject.SetActive(false);
        }

        public List<ResourceNode> GetSelectedNodes() =>
            _selectedResourceNodes;

        public void SetSelectMode(SelectionMode selectionMode)
        {
            _currentMode = selectionMode;
            SelectionModeChanged?.Invoke(_currentMode);
        }

        private void Update()
        {
            if (_currentMode == SelectionMode.None)
                return;

            if (_selection)
                DrawSelectedArea();
        }

        private void OnSelectCanceled(InputAction.CallbackContext inputValue)
        {
            SetSelectMode(SelectionMode.None);
            ClearSelectedArea();
        }

        private void OnMousePressed(InputAction.CallbackContext inputValue)
        {
            Vector2 pointerPosition = _playerInput.Selection.PointerPosition.ReadValue<Vector2>();
            _startDragPosition = GetMouseWorldPosition(pointerPosition);

            _selection = true;
            _selectedArea.gameObject.SetActive(true);

            ClearAllSelectedObjects();
        }

        private void OnMouseRaised(InputAction.CallbackContext inputValue)
        {
            if (_currentMode != SelectionMode.None)
            {
                SelectObjects();
                
                if (_selectedResourceNodes.Count > 0) 
                    ResourceNodeSelected?.Invoke(_currentMode);
            }
            
            ClearSelectedArea();
        }

        private void SelectObjects()
        {
            Vector3 centerArea = new Vector3(_selectedArea.localScale.x * .5f, 5f, _selectedArea.localScale.z * .5f);

            bool isSingleTarget = false;

            if (centerArea.x < .5f)
            {
                centerArea.x = .45f;
                isSingleTarget = true;
            }

            if (centerArea.z < .5f)
            {
                centerArea.z = .45f;
                isSingleTarget = true;
            }
            
            if (isSingleTarget)
            {
                Vector2 screenPosition = _playerInput.Selection.PointerPosition.ReadValue<Vector2>();
                Ray ray = _camera.ScreenPointToRay(screenPosition);

                if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, _selectionObjectsMask))
                {
                    if (raycastHit.collider.TryGetComponent(out ResourceNode node)) 
                        _selectedResourceNodes.Add(node);
                }
                
                return;
            }
            
            Collider[] hitColliders = Physics.OverlapBox(_selectionCenterPosition, 
                centerArea, Quaternion.identity, _selectionObjectsMask);

            foreach (Collider collider in hitColliders)
            {
                if (collider.TryGetComponent(out ResourceNode node)) 
                    _selectedResourceNodes.Add(node);
            }
        }

        private void ClearAllSelectedObjects() => 
            _selectedResourceNodes.Clear();

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
            
            // нижний левый угол (начало)
            // Vector3 lowerLeft = new Vector3(
            //     Mathf.FloorToInt(Mathf.Min(_startDragPosition.x, currentPosition.x)),
            //     0.1f,
            //     Mathf.FloorToInt(Mathf.Min(_startDragPosition.z, currentPosition.z))
            // );
            //
            // // правый верхний
            // Vector3 upperRight = new Vector3(
            //     Mathf.FloorToInt(Mathf.Max(_startDragPosition.x, currentPosition.x)),
            //     0.1f,
            //     Mathf.FloorToInt(Mathf.Max(_startDragPosition.z, currentPosition.z))
            // );

            // центр зоны выделения
            _selectionCenterPosition = new Vector3(
                lowerLeft.x + ((upperRight.x - lowerLeft.x) * .5f),
                2f,
                lowerLeft.z + ((upperRight.z - lowerLeft.z) * .5f)
            );

            _selectedArea.position = lowerLeft;
            _selectedArea.localScale = upperRight - lowerLeft;
        }

        private void ClearSelectedArea()
        {
            _selection = false;
            _selectedArea.localScale = Vector3.zero;
            _selectedArea.gameObject.SetActive(false);
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