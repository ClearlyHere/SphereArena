using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Course_Library.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody _playerRb;
        private PlayerInputs _playerControl;
        private InputAction _playerMove;
        private InputAction _playerFire;
        
        [SerializeField] private GameObject powerRing;
        [SerializeField] private GameObject rocketRing;
        [FormerlySerializedAs("missile")] [SerializeField] private GameObject missilePrefab;
        
        private Vector3 _moveDirection = Vector3.zero;
        private const float MoveSpeed = 100f;
        
        private bool _isPoweredUp;
        private const float PowerUpStrength = 500f;
        private float _powerUpDuration = 7;

        private bool _hasRockets;
        private bool _canShoot;
        private float _rocketsDuration = 5f;
        private float _rocketsRateFire = 0.5f;
        
        
        private void Awake()
        {
            _playerControl = new PlayerInputs();
            _playerRb = GetComponent<Rigidbody>();
        }

        private void OnEnable()
        {
            _playerMove = _playerControl.Player.Move;
            _playerMove.Enable();

            _playerFire = _playerControl.Player.Fire;
            _playerFire.Enable();
            _playerFire.performed += Fire;
        }

        private void Update()
        {
            PlayerMove();
        }

        private void PlayerMove()
        {
            // Player Movement
            _moveDirection = _playerMove.ReadValue<Vector3>();
            _playerRb.AddForce(Vector3.forward * (MoveSpeed * _moveDirection.z));
            _playerRb.AddForce(Vector3.right * (MoveSpeed * _moveDirection.x));
            
            // Rings Movement
            FollowPlayerRing(powerRing);
            FollowPlayerRing(rocketRing);
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
            else if (other.CompareTag("Rockets"))
            {
                _hasRockets = true;
                _canShoot = true;
                Destroy(other.gameObject);
                rocketRing.gameObject.SetActive(true);
                StartCoroutine(RocketsCountdownRoutine());
            }
        }

        IEnumerator PowerUpCountdownRoutine()
        {
            yield return new WaitForSeconds(_powerUpDuration);
            powerRing.gameObject.SetActive(false);
            _isPoweredUp = false;
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Enemy") && _isPoweredUp)
            {
                PushEnemyAway(collision);
            }
        }
        
        IEnumerator RocketsCountdownRoutine()
        {
            yield return new WaitForSeconds(_rocketsDuration);
            rocketRing.SetActive(false);
            _hasRockets = false;
        }

        private IEnumerator RocketsFireRateCountdownRoutine()
        {
            yield return new WaitForSeconds(_rocketsRateFire);
            _canShoot = true;
        }

        private void Fire(InputAction.CallbackContext context)
        {
            if (_hasRockets && _canShoot)
            {
                Vector3 playerPosition = transform.position;
                Vector3 topOfPlayerPosition = playerPosition + new Vector3(0, 1, 0);
                
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject enemy in gameObjects)
                {
                    GameObject missile = Instantiate(missilePrefab, topOfPlayerPosition , missilePrefab.transform.rotation);
                    RocketHoming missileScript = missile.GetComponent<RocketHoming>();
                    missileScript.SetEnemyTarget(enemy);
                }
                _canShoot = false;
                StartCoroutine(RocketsFireRateCountdownRoutine());
            }
        }

        private void PushEnemyAway(Collision collision)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * PowerUpStrength, ForceMode.Impulse);
        }

        private void FollowPlayerRing(GameObject ring)
        {
            Vector3 position = transform.position;
            ring.gameObject.transform.position =
                new Vector3(position.x, position.y - 0.415f, position.z);
        }
    }
}