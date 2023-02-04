using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Code.Logic.Camera
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTransform;
        
        [Space(4)]
        [Header("Camera movement settings")] 
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _acceleration = 10f;
        [SerializeField] private float _dumping = 15f;
        
        [Space(4)]
        [Header("Camera zoom settings")]
        [SerializeField] private float _minHeight = 8f;
        [SerializeField] private float _maxHeight = 40f;
        [SerializeField] private float _stepSize = 2f;
        [SerializeField] private float _zoomDamping = 7.5f;
        [SerializeField] private float _zoomSpeed = 2f;
        
        [Space(4)]
        [Header("Camera rotation settings")]
        [SerializeField] private float _maxRotationSpeed = 1f;
        
        private CameraControl _cameraActions;
        private InputAction _movementAction;

        // позиция, за которой мы хотим "следовать"
        private Vector3 _targetPosition;
        private float _currentSpeed;
        private float _zoomHeight;

        private Vector3 _cameraLastPosition;
        private Vector3 _horizontalVelocity;

        public void Init()
        {
            _cameraActions = new CameraControl();
            _movementAction = _cameraActions.Camera.Movement;

            // вынести события в другой класс типа инпут хендлера
            _cameraActions.Camera.Rotate.performed += UpdateRotation;
            _cameraActions.Camera.Zoom.performed += HeightChanged;
            _cameraActions.Camera.Enable();
            
            _cameraLastPosition = transform.position;
            _zoomHeight = _cameraTransform.localPosition.y;
            //_cameraTransform.LookAt(transform);
        }

        private void OnDisable()
        {
            _cameraActions.Camera.Rotate.performed -= UpdateRotation;
            _cameraActions.Camera.Zoom.performed -= HeightChanged;
            _cameraActions.Disable();
        }

        private void LateUpdate()
        {
            CalculateInputMovement();
            
            UpdateVelocity();
            UpdateCameraPosition();
            UpdatePosition();
        }

        private void CalculateInputMovement()
        {
            Vector3 inputValue = _movementAction.ReadValue<Vector2>().x * GetRightCameraDirection()
                                 + _movementAction.ReadValue<Vector2>().y * GetForwardCameraDirection();

            inputValue = inputValue.normalized;

            if (inputValue.sqrMagnitude > 0.1f)
                _targetPosition += inputValue;
        }

        private Vector3 GetRightCameraDirection()
        {
            Vector3 right = _cameraTransform.right;
            right.y = 0;
            return right;
        }
        
        private Vector3 GetForwardCameraDirection()
        {
            Vector3 forward = _cameraTransform.forward;
            forward.y = 0;
            return forward;
        }

        private void UpdatePosition()
        {
            if (_targetPosition.sqrMagnitude > 0.1f)
            {
                // постепенно повышаем нашу скорость до максимальной, через ускорение
                _currentSpeed = Mathf.Lerp(_currentSpeed, _maxSpeed, Time.deltaTime * _acceleration);
                transform.position += _targetPosition * (_currentSpeed * Time.deltaTime);
            }
            else
            {
                // если целевая позиция почти рядом, то замедляем нашу скорость
                _horizontalVelocity = Vector3.Lerp(_horizontalVelocity, Vector3.zero, Time.deltaTime * _dumping);
                transform.position += _horizontalVelocity * Time.deltaTime;
            }
            
            _targetPosition = Vector3.zero;
        }

        private void UpdateVelocity()
        {
            // получаем горизонтальное смещение за один кадр
            _horizontalVelocity = (transform.position - _cameraLastPosition) / Time.deltaTime;
            _horizontalVelocity.y = 0;
            // обновляем последнюю позицию
            _cameraLastPosition = transform.position;
        }
        
        private void UpdateRotation(InputAction.CallbackContext inputValue)
        {
            if (Mouse.current.middleButton.isPressed == false)
                return;

            float value = inputValue.ReadValue<Vector2>().x;
            float y = value * _maxRotationSpeed + transform.rotation.eulerAngles.y;
            
            transform.rotation = Quaternion.Euler(0f, y, 0f);
        }

        private void UpdateCameraPosition()
        {
            Vector3 cameraPosition = _cameraTransform.localPosition;
            
            Vector3 desiredZoomTarget = new Vector3(
                cameraPosition.x,
                _zoomHeight,
                cameraPosition.z);

            desiredZoomTarget -= _zoomSpeed * (_zoomHeight - cameraPosition.y) * Vector3.forward;

            _cameraTransform.localPosition = Vector3.Lerp(
                cameraPosition, 
                desiredZoomTarget, 
                _zoomDamping * Time.deltaTime);
            //_cameraTransform.LookAt(transform);
        }
        
        private void HeightChanged(InputAction.CallbackContext inputValue)
        {
            float value = -inputValue.ReadValue<Vector2>().y / 100f;

            if (Mathf.Abs(value) > 0.1f)
            {
                // желаемая высота которой мы хотим достичь
                _zoomHeight = _cameraTransform.localPosition.y + value * _stepSize;
                _zoomHeight = Mathf.Clamp(_zoomHeight, _minHeight, _maxHeight);
            }
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(transform.position, 1f);
        }
    }
}