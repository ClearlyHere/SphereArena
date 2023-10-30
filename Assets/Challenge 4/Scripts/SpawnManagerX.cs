using UnityEngine;
using Random = UnityEngine.Random;

namespace Challenge_4.Scripts
{
    public class SpawnManagerX : MonoBehaviour
    {
        public GameObject enemyPrefab;
        public GameObject powerupPrefab;

        private const float SpawnRangeX = 10;
        private const float SpawnZMin = 15; // set min spawn Z
        private const float SpawnZMax = 25; // set max spawn Z

        public int enemyCount;
        public int waveCount = 1;
        
        public GameObject player;
        private Rigidbody _playerRb;

        private void Start()
        {
            _playerRb = player.GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemyCount == 0)
            {
                SpawnEnemyWave(waveCount);
            }

        }

        // Generate random spawn position for powerups and enemy balls
        Vector3 GenerateSpawnPosition ()
        {
            float xPos = Random.Range(-SpawnRangeX, SpawnRangeX);
            float zPos = Random.Range(SpawnZMin, SpawnZMax);
            return new Vector3(xPos, 0, zPos);
        }


        void SpawnEnemyWave(int enemiesToSpawn)
        {
            Vector3 powerupSpawnOffset = new Vector3(0, 0, -15); // make powerups spawn at player end

            // If no powerups remain, spawn a powerup
            if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) // check that there are zero powerups
            {
                Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
            }

            // Spawn number of enemy balls based on wave number
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            }

            waveCount++;
            ResetPlayerPosition(); // put player back at start

        }

        // Move player back to position in front of own goal
        private void ResetPlayerPosition ()
        {
            player.transform.position = new Vector3(0, 1, -7);
            _playerRb.velocity = Vector3.zero;
            _playerRb.angularVelocity = Vector3.zero;
        }

    }
}
