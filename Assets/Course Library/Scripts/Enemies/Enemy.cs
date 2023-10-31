using UnityEngine;

namespace Course_Library.Scripts.Enemies
{
    public class Enemy : MonoBehaviour
    {
        protected GameObject Player;
        protected Rigidbody EnemyRb;
        protected SpawnManager SpawnManager;
        protected const float MovementSpeed = 80f;
        private const float FallBound = -10f;
        
        // Start is called before the first frame update
        protected virtual void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            SpawnManager = GameObject.FindGameObjectWithTag("SpawnManager").GetComponent<SpawnManager>();
            EnemyRb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            Movement();
            DestroyOnFall();
        }

        protected virtual void Movement()
        {
            Vector3 lookDirection = (Player.transform.position - transform.position).normalized ;
            EnemyRb.AddForce(lookDirection * MovementSpeed);
        }

        protected void DestroyOnFall()
        {
            if (transform.position.y < FallBound)
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            SpawnManager.ReduceEnemyCount();
        }
    }
}
