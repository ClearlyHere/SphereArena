using UnityEngine;
using Random = UnityEngine.Random;

namespace Course_Library.Scripts
{
    public class SpawnManager : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject strongEnemyPrefab;
        [SerializeField] private GameObject bossPrefab;
        
        [SerializeField] private GameObject powerUp;
        [SerializeField] private GameObject rocketsPickup;
        [SerializeField] private GameObject smashPickup;

        private const float SpawnRange = 9;
        private int _waveNumber = 1;
        private int _strongEnemySpawns = 1;
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
        
        private void InstantiateStrongEnemy()
        {
            Instantiate(strongEnemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
        }

        private void InstantiateBoss()
        {
            Instantiate(bossPrefab, GenerateRandomPosition() + new Vector3(0, 2, 0), bossPrefab.transform.rotation);
        }

        private void InstantiatePowerUp()
        {
            Instantiate(powerUp, GenerateRandomPosition(), powerUp.transform.rotation);
        }

        private void InstantiateRocketsPickup()
        {
            Instantiate(rocketsPickup, GenerateRandomPosition(), powerUp.transform.rotation);
        }

        private void InstantiateSmashPickup()
        {
            Instantiate(smashPickup, GenerateRandomPosition(), smashPickup.transform.rotation);
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
            switch (waveNumber)
            {
                case 1:
                    InstantiateEnemy();
                    _enemyCount++;
                    break;
                case 10:
                    InstantiateBoss();
                    _enemyCount++;
                    break;
                default:
                    if (waveNumber > 10)
                    {
                        // Nothing more! You beat the boss hurray!
                        _enemyCount++;
                    }
                    else
                    {
                        SpawnStrongEnemiesLoop(_strongEnemySpawns);
                        SpawnEnemiesLoop(waveNumber);
                    }
                    break;
            }

            if (waveNumber is > 3 or < 11)
            {
                InstantiateSmashPickup();
            }

            if (waveNumber is > 5 or < 11)
            {
                InstantiateRocketsPickup();
            }

            IncreaseStrongOnWaveModulo(waveNumber, 3);
            InstantiatePowerUp();
            _waveNumber++;
        }

        public void ReduceEnemyCount()
        {
            _enemyCount--;
        }

        private void IncreaseStrongOnWaveModulo(int waveNumber, int modulo)
        {
            if (waveNumber % modulo == 0)
            {
                _strongEnemySpawns++;
            }
        }

        private void SpawnStrongEnemiesLoop(int strongEnemiesNumber)
        {
            for (int k = 0; k < strongEnemiesNumber; k++)
            {
                InstantiateStrongEnemy();
                _enemyCount++;
            }
        }

        private void SpawnEnemiesLoop(int waveNumber)
        {
            for (int i = 0 + _strongEnemySpawns; i < waveNumber; i++)
            {
                InstantiateEnemy();
                _enemyCount++;
            }
        }
    }
}
