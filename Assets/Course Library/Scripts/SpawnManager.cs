using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;

    private const float SpawnRange = 9;
    // Start is called before the first frame update
    private void Start()
    {
        InstantiateEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InstantiateEnemy()
    {
        Instantiate(enemyPrefab, GenerateRandomPosition(), enemyPrefab.transform.rotation);
    }

    private Vector3 GenerateRandomPosition()
    {
        float spawnPosX = Random.Range(-SpawnRange, SpawnRange);
        float spawnPosZ = Random.Range(-SpawnRange, SpawnRange);
        Vector3 randomPos = new Vector3(spawnPosX, 0, spawnPosZ);
        return randomPos;
    }
}
