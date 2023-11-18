using UnityEngine;

namespace Challenge_4.Scripts
{
    public class EnemyX : MonoBehaviour
    {
        public float speed = 100;
        private static float _speedModifier = 0.8f;
        private Rigidbody _enemyRb;
        private GameObject _playerGoal;

        // Start is called before the first frame update
        void Start()
        {
            _playerGoal = GameObject.FindGameObjectWithTag("PlayerGoal");
            _enemyRb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            // Set enemy direction towards player goal and move there
            Vector3 lookDirection = (_playerGoal.transform.position - transform.position).normalized;
            _enemyRb.AddForce(lookDirection * (speed * Time.deltaTime * _speedModifier));

        }

        private void OnCollisionEnter(Collision other)
        {
            // If enemy collides with either goal, destroy it
            if (other.gameObject.name == "Enemy Goal")
            {
                Destroy(gameObject);
            } 
            else if (other.gameObject.name == "Player Goal")
            {
                Destroy(gameObject);
            }

        }

        public void IncreaseSpeedModifier()
        {
            _speedModifier += 0.2f;
        }
    }
}
