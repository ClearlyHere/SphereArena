using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Course_Library.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _playerRb;
        private PlayerInputs _playerControl;
        private InputAction _playerMove;
        [SerializeField] private GameObject powerRing;
        
        private Vector3 _moveDirection = Vector3.zero;
        private const float MoveSpeed = 100f;
        
        private bool _isPoweredUp;
        private const float PowerUpStrength = 100f;

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
            FollowPowerRing();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PowerUp"))
            {
                _isPoweredUp = true;
                Destroy(other.gameObject);
                powerRing.gameObject.SetActive(true);
                StartCoroutine(PowerUpCountdownRoutine());
            }
        }

        IEnumerator PowerUpCountdownRoutine()
        {
            yield return new WaitForSeconds(7);
            powerRing.gameObject.SetActive(false);
            _isPoweredUp = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy") && _isPoweredUp)
            {
                Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                Vector3 awayFromPlayer = (collision.gameObject.transform.position - transform.position);
                enemyRb.AddForce(awayFromPlayer * PowerUpStrength, ForceMode.Impulse);
            }
        }

        private void FollowPowerRing()
        {
            Vector3 position = transform.position;
            powerRing.gameObject.transform.position =
                new Vector3(position.x, position.y - 0.415f, position.z);
        }
    }
}