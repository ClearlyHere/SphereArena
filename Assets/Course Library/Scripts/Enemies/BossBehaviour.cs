using System.Collections;
using UnityEngine;

namespace Course_Library.Scripts.Enemies
{
    public class BossBehaviour : Enemy
    {
        [SerializeField] private GameObject miniEnemy;
        [SerializeField] private GameObject missilePrefab;

        private bool _isPerformingAction;
        private float _actionCooldown = 5f;
        private float _timer;
        
        private float _bossSpeedModifier = 5f;
        private const float SpawnRange = 4f;

        protected override void Start()
        {
            base.Start();
            StartCoroutine(ActionCoroutine());
        }

        protected override void Update()
        {
            base.Update();  
            if (_isPerformingAction)
            {
                _timer += Time.deltaTime;
                if (_timer >= _actionCooldown)
                {
                    _isPerformingAction = false;
                    _timer = 0f;
                }
            }
        }

        private IEnumerator ActionCoroutine()
        {
            while (true)
            {
                if (!_isPerformingAction)
                {
                    int randomAction = Random.Range(1, 4);
                    PerformAction(randomAction);
                    _isPerformingAction = true;
                    yield return new WaitForSeconds(_actionCooldown);
                }
                else
                {
                    yield return null;
                }
            }
        }

        private void PerformAction(int action)
        {
            switch (action)
            {
                case 1:
                    _bossSpeedModifier = 5f;
                    SpawnMiniEnemies(5);
                    break;
                case 2:
                    _bossSpeedModifier = 5f;
                    InstantiateProjectiles(5);
                    break;
                case 3:
                    _bossSpeedModifier = 50f;
                    break;
            }
        }

        protected override void Movement()
        {
            var lookDirection = (Player.transform.position - transform.position).normalized ;
            EnemyRb.AddForce(lookDirection * (MovementSpeed * _bossSpeedModifier));
        }

        private void SpawnMiniEnemies(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                InstantiateMiniEnemy();
            }
        }

        private void InstantiateMiniEnemy()
        {
            Instantiate(miniEnemy, GenerateRandomPosition(), miniEnemy.transform.rotation);
        }

        private void InstantiateProjectiles(int amount)
        {
            var bossPosition = transform.position;
            var topOfBossPosition = bossPosition + new Vector3(0, 3, 0);

            for (int i = 0; i < amount; i++)
            {
                Instantiate(missilePrefab, topOfBossPosition , missilePrefab.transform.rotation);
            }
        }
        
        private Vector3 GenerateRandomPosition()
        {
            var spawnPosX = Random.Range(-SpawnRange, SpawnRange);
            var spawnPosZ = Random.Range(-SpawnRange, SpawnRange);
            var randomPos = new Vector3(spawnPosX, 0, spawnPosZ) + transform.position;
            return randomPos;
        }
    }
}
