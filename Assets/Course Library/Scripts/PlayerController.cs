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
        private InputAction _playerFire;
        private InputAction _playerSmash;
        
        [SerializeField] private GameObject powerRing;
        [SerializeField] private GameObject rocketRing;
        [SerializeField] private GameObject smashRing;
        [SerializeField] private GameObject missilePrefab;
        
        private Vector3 _moveDirection = Vector3.zero;
        private const float MoveSpeed = 100f;
        
        private bool _isPoweredUp;
        private const float PowerUpStrength = 500f;
        private float _powerUpDuration = 7;

        private bool _hasRockets;
        private bool _canShoot;
        private float _rocketsDuration = 5f;
        private float _rocketsRateFire = 0.5f;

        private bool _hasSmash;
        private float _smashDuration = 3f;
        private float _smashWindupDuration = 0.5f;
        private const float SmashDownForce = 2000f;
        private const float SmashExplosion = 1500f;
        private const float SmashRadius = 5f;
        private const float TriggerForce = 10f;
        private const float VectorWindupModifier = 0.2f;
        
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
            
            _playerSmash = _playerControl.Player.Smash;
            _playerSmash.Enable();
            _playerSmash.performed += Smash;
        }

        private void OnDisable()
        {
            _playerMove.Disable();
            _playerFire.Disable();
            _playerSmash.Disable();
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
            FollowPlayerRing(powerRing, -0.415f);
            FollowPlayerRing(rocketRing, -0.415f);
            FollowPlayerRing(smashRing, 0);
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
            else if (other.CompareTag("Smash"))
            {
                _hasSmash = true;
                Destroy(other.gameObject);
                smashRing.gameObject.SetActive(true);
                StartCoroutine(SmashCountdownRoutine());
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

            if (collision.gameObject.CompareTag("Respawn"))
            {
                ExplosiveSmashForce(collision);
            }
        }
        
        private IEnumerator RocketsCountdownRoutine()
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

        private IEnumerator SmashCountdownRoutine()
        {
            yield return new WaitForSeconds(_smashDuration);
            smashRing.SetActive(false);
            _hasSmash = false;
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

        private void Smash(InputAction.CallbackContext context)
        {
            if (_hasSmash)
            {
                _hasSmash = false;
                smashRing.SetActive(false);
                StartCoroutine(SmashWindupDuration());
            }
        }

        IEnumerator SmashWindupDuration()
        {
            for (float t = 0f; t < _smashWindupDuration; t += Time.deltaTime)
            {
                var transform1 = transform;
                var playerPosition = transform1.position;
                transform1.position = new Vector3(playerPosition.x, playerPosition.y + VectorWindupModifier, playerPosition.z);
                yield return null;
            }

            _playerRb.AddForce(Vector3.down * SmashDownForce, ForceMode.Impulse);
        }

        private void ExplosiveSmashForce(Collision collision)
        {
            if (collision.relativeVelocity.magnitude >= TriggerForce)
            {
                var surroundingEnemies = Physics.OverlapSphere(transform.position, SmashRadius);
                foreach (var enemy in surroundingEnemies)
                {
                    if (enemy.CompareTag("Enemy"))
                    {
                        var enemyRb = enemy.GetComponent<Rigidbody>();
                        enemyRb.AddExplosionForce(SmashExplosion, transform.position, SmashRadius, 1, ForceMode.Impulse);
                    }
                }
            }
        }

        private void PushEnemyAway(Collision collision)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * PowerUpStrength, ForceMode.Impulse);
        }

        private void FollowPlayerRing(GameObject ring, float offsetYPosition)
        {
            Vector3 position = transform.position;
            ring.gameObject.transform.position =
                new Vector3(position.x, position.y + offsetYPosition, position.z);
        }
    }
}