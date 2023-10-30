using UnityEngine;

namespace Course_Library.Scripts
{
    public class EnemyMovement : MonoBehaviour
    {
        private GameObject _player;
        private Rigidbody _enemyRb;
        private const float MovementSpeed = 80f;
        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindGameObjectWithTag("Player");
            _enemyRb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            Movement();
        }

        void Movement()
        {
            Vector3 lookDirection = (_player.transform.position - transform.position).normalized ;
            _enemyRb.AddForce(lookDirection * MovementSpeed);
        }
    }
}
