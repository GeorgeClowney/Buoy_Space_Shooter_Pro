using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// The Spwan Manager Script controls spawning of the player, enemies and powerups;
public class SpawnManager : MonoBehaviour
{
    //_playerInstance is the current _player set in the PlayerProfile script
    [SerializeField]
    private GameObject _playerInstance;
    //_enemyContainer is a empty gameObject inside of Spawn Manager that stores the spawned enemies
    [SerializeField]
    private GameObject _enemyPrefab, _enemyContainer;
    [SerializeField]
    private GameObject[] powerups;
    //posToSpawn is a random point at the top of the screen
    private Vector3 _posToSpawn;
    //_stopSpawning prevents the SpawnManager from spawning the enemies and powerups 
    private bool _stopSpawning;

    private int _powerupID;
    /*Powerups are stored in a array and are called with _powerupID
    Powerups in order are:
    0. Triple Shot
    1. Speed Boost
    2. Shield
    3. Ammo Pack
    4. Repair Kit
    5. Homing Lasers
    */

    //Store the current state of SpawnManager as an _instance
    private SpawnManager _instance;
    //Make the SpawnManger global
    void MakeSingleton()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Awake()
    {
        MakeSingleton();
    }
    //Start Spawning Enemys and powerups
    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerupRoutine());
    }
    //Enemy Spawn Routine
    IEnumerator SpawnRoutine()
    {
        //WhileLoop waits 3 seconds after it is called 
        yield return new WaitForSeconds(3f);
        //Stop Spawning needs to be true for the enemies to start spawning
        while (!_stopSpawning)
        {
            _posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            GameObject newEnemy = Instantiate(_enemyPrefab, _posToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5.0f);
        }
    }
    //Powerup Spawn Routine
    IEnumerator SpawnPowerupRoutine()
    {
        //WhileLoop waits 3 seconds after it is called 
        yield return new WaitForSeconds(3f);
        while (!_stopSpawning)
        {
            _posToSpawn = new Vector3(Random.Range(-8f, 8f), 7, 0);
            //randomPowerUp picks a powerup at random
            int randomPowerUp = Random.Range(0, 6);
            //after picking a random number within the range of the powerups array spawn a powerup with the corresponding number
            Instantiate(powerups[randomPowerUp], _posToSpawn, Quaternion.identity);
            //Wait 3-7 seconds before spawning another powerup
            yield return new WaitForSeconds(Random.Range(3, 8));
        }  
    }
    //If the player is dead stop spawning
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
    private void Update()
    {
        RestartGame();
    }
    //If the game is restarted destory the current Spawn Manager
    private void RestartGame()
    {
        GameManager gm = gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (gm == null)
        {
            Debug.LogError("Game Manager is Null (SpawnManager)");
        }
        else if (gm != null && gm.gameRestarted)
        {
            Destroy(this.gameObject);
        }
    }
}
