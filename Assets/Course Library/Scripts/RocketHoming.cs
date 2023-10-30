using System.Collections;
using UnityEngine;

namespace Course_Library.Scripts
{
    public class RocketHoming : MonoBehaviour
    {
        private PlayerController _playerController;
        private GameObject _enemy;
        private Rigidbody _enemyRb;
        private Rigidbody _missileRb;

        private const float MissileSpeed = 20f;
        private const float MissileStrength = 1000f;
        private float _missileDuration = 1.5f;
        private bool _isEnemyNull;


        private void Start()
        {
            _missileRb = GetComponent<Rigidbody>();
            _enemyRb = _enemy.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (_enemy == null)
            {
                Destroy(gameObject);
            }
            else
            {
                TargetEnemy(_enemy);
            }
        }

        private void TargetEnemy(GameObject enemy)
        {
            var enemyPosition = enemy.transform.position;
            var transform1 = transform;
            var missilePosition = transform1.position;
            var enemyDirection = (enemyPosition - missilePosition).normalized;
            
            _missileRb.velocity = transform1.forward * MissileSpeed;
            Quaternion rotation = Quaternion.LookRotation(enemyDirection);
            _missileRb.MoveRotation(rotation);
        }

        public void SetEnemyTarget(GameObject enemy)
        {
            _enemy = enemy;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                var enemyPosition = other.transform.position;
                var missilePosition = transform.position;
                var awayFromMissile = (enemyPosition - missilePosition).normalized;
                _enemyRb.AddForce(awayFromMissile * MissileStrength, ForceMode.Impulse);
                
                StartCoroutine(MissileDuration());
            }
        }
        
        IEnumerator MissileDuration()
        {
            yield return new WaitForSeconds(_missileDuration);
            Destroy(gameObject);
        }
    }
}

