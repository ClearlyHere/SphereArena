using UnityEngine;
using UnityEngine.InputSystem;

namespace Course_Library.Scripts
{
    public class CameraControl : MonoBehaviour
    {
        private PlayerInputs _playerControls;
        private InputAction _cameraMove;
        private float _moveDirection;
        private const float RotationSpeed = 20f;

        private void Awake()
        {
            _playerControls = new PlayerInputs();
        }
        private void OnEnable()
        {
            _cameraMove = _playerControls.Player.Camera;
            _cameraMove.Enable();
        }

        private void OnDisable()
        {
            _cameraMove.Disable();
        }
    
        // Update is called once per frame
        void Update()
        {
            CameraMovement();
        }

        private void CameraMovement()
        {
            _moveDirection = _cameraMove.ReadValue<float>();
            transform.Rotate(Vector3.up * (RotationSpeed * _moveDirection * Time.deltaTime));
        }
    }
}
