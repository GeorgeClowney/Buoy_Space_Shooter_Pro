using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Asteroid is a type of enemy that the player can destroy
public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private float _speed = 19.0f;
    //_boomPrefab is a explosion prefab
    [SerializeField]
    private GameObject _boomPrefab;
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.LogError("SpawnManager in Asteroid is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Spin();
    }
    //Rotate the Asteroid
    private void Spin()
    {
        transform.Rotate(Vector3.forward * _speed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser")
        {
            Instantiate(_boomPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, 0.17f);
        }

    }
}
