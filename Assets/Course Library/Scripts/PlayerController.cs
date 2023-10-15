﻿using UnityEngine;
using UnityEngine.InputSystem;

namespace Course_Library.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _playerRb;
        private PlayerInputs _playerControl;
        private InputAction _playerMove;
        
        private Vector3 _moveDirection = Vector3.zero;
        private const float MoveSpeed = 100f;

        private void Awake()
        {
            _playerControl = new PlayerInputs();
            _playerRb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _playerMove = _playerControl.Player.Move;
            _playerMove.Enable();
        }

        private void Update()
        {
            PlayerMove();
        }

        private void PlayerMove()
        {
            _moveDirection = _playerMove.ReadValue<Vector3>();
            _playerRb.AddForce(Vector3.forward * (MoveSpeed * _moveDirection.z));
            _playerRb.AddForce(Vector3.right * (MoveSpeed * _moveDirection.x));
        }
    }
}