using UnityEngine;

namespace Course_Library.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        private GameObject _player;
        private Rigidbody _enemyRb;
        private SpawnManager _spawnManager;
        private const float MovementSpeed = 80f;

        private const float FallBound = -10f;
        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _spawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
            _enemyRb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
            DestroyOnFall();
        }

        void Movement()
        {
            Vector3 lookDirection = (_player.transform.position - transform.position).normalized ;
            _enemyRb.AddForce(lookDirection * MovementSpeed);
        }

        void DestroyOnFall()
        {
            if (transform.position.y < FallBound)
            {
                _spawnManager.ReduceEnemyCount();
                Destroy(gameObject);
            }
        }
    }
}
