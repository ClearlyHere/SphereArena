using System.Collections;
using UnityEngine;

namespace Course_Library.Scripts.Enemies
{
    public class Projectile : RocketHoming
    {
        private const float MissileSpeed = 5f;
        private const float MissileStrength = 100f;
        private float _projectileDuration = 5f;
        
        private GameObject _player;
        private Rigidbody _playerRb;

        private void Start()
        {
            MissileRb = GetComponent<Rigidbody>();
            _player = GameObject.FindWithTag("Player");
            _playerRb = _player.GetComponent<Rigidbody>();
            StartCoroutine(MissileDuration());
        }
        
        void FixedUpdate()
        {
            if (_player == null)
            {
                Destroy(gameObject);
            }
            else
            {
                TargetEnemy();
            }
        }
        
        protected override void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var enemyPosition = other.transform.position;
                var missilePosition = transform.position;
                var awayFromMissile = (enemyPosition - missilePosition).normalized;
                _playerRb.AddForce(awayFromMissile * MissileStrength, ForceMode.Impulse);
            }
        }

        private void TargetEnemy()
        {
            var transform1 = transform;
            var playerPosition = _player.transform.position;
            var missilePosition = transform1.position;
            var enemyDirection = (playerPosition - missilePosition).normalized;
            
            MissileRb.velocity = transform1.forward * MissileSpeed;
            Quaternion rotation = Quaternion.LookRotation(enemyDirection);
            MissileRb.MoveRotation(rotation);
        }
        
        protected override IEnumerator MissileDuration()
        {
            yield return new WaitForSeconds(_projectileDuration);
            Destroy(gameObject);
        }
    }
}