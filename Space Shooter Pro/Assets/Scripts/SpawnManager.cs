using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab, _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    private Vector3 posToSpawn;
    private bool _stopSpawning;
    private int _powerupID;
    void Start()
    {
       
    }
    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
    IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            int randomPowerUp = Random.Range(0, 6);
            print(randomPowerUp);
             Instantiate(powerups[randomPowerUp], posToSpawn, Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(3, 8));
        }  
    }
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
