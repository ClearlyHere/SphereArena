using UnityEngine;

namespace Course_Library.Scripts
{
    public class StrongEnemy : Enemy
    {
        private Rigidbody _playerRb;
        private const float EnemyStrength = 300f;
        protected override void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            _playerRb = Player.GetComponent<Rigidbody>();
            SpawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
            EnemyRb = GetComponent<Rigidbody>();
        }
        
        protected override void Update()
        {
            Movement();
            DestroyOnFall();
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                var enemyPos = transform.position;
                var playerPos = other.transform.position;
                var awayFromPlayer = (playerPos - enemyPos).normalized;
                _playerRb.AddForce(awayFromPlayer * EnemyStrength, ForceMode.Impulse);
            }
        }
    }
}
