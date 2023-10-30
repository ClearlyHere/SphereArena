using UnityEngine;
using Random = UnityEngine.Random;

namespace Course_Library.Scripts
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject powerUp;

        private const float SpawnRange = 9;
        private int _waveNumber = 1;
        private int _enemyCount;

        private void Start()
        {
            StartEnemyWave(_waveNumber);
        }

        // Update is called once per frame
        void Update()
        {
            if (_enemyCount == 0)
            {
                StartEnemyWave(_waveNumber);
            }
        }

        private void InstantiateEnemy()
        {
            Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
        }

        private void InstantiatePowerUp()
        {
            Instantiate(powerUp, GenerateRandomPosition(), powerUp.transform.rotation);
        }

        private Vector3 GenerateRandomPosition()
        {
            float spawnPosX = Random.Range(-SpawnRange, SpawnRange);
            float spawnPosZ = Random.Range(-SpawnRange, SpawnRange);
            Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
            return randomPos;
        }

        private void StartEnemyWave(int waveNumber)
        {
            InstantiatePowerUp();
            for (int i = 0; i < waveNumber; i++)
            {
                InstantiateEnemy();
                _enemyCount++;
            }

            _waveNumber++;
        }

        public void ReduceEnemyCount()
        {
            _enemyCount--;
        }
    }
}
