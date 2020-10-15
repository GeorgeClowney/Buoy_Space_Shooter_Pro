using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Basic Enemy script
//This Enemy moves from top to bottom and shots in front of its self
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _fireRate = 3.0f, _canFire = -1;
    private Animator _enemyanim;
    private bool _isDead;
    //The _laserPrefab is set in the inspecter
    [SerializeField]
    private GameObject _laserPrefab;

    private Player _player;
    private GameManager _gm;

    private Rigidbody2D _rb;
    void Start()
    {
        _gm = GameObject.Find("Game Manager").GetComponent<GameManager>();
        if (_gm == null)
        {
            Debug.LogError("Game Manager is Null (Enemy)");
        }
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is Null(Enemy)");
        }
        _enemyanim = GetComponent<Animator>();
        if(_enemyanim == null) 
        {
            Debug.LogError("The Enemy can't find it's Animator component");
        }
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
            Debug.LogError("The Enemy can't find it's Rigidbody2D Component");
        }
    }
    void Update()
    {
        if (!_isDead && _gm.isGameOver == false)
        {
            CalculateMovement();
            Shooting();
        }
        //Check if the game is over and destory every enemy
        else if(_gm.isGameOver)
        {
            Destroy(this.gameObject);
        }
    }
    private void Shooting()
    {
        if (Time.time > _canFire)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            //The lasers need to be set as enemy lasers so that they shoot downwards
            GameObject eLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = eLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
            //Can cause bugs ^
            //If it can not find lasers for any reason the game will crash
            //Current fix is to only run the Shooting function if the game is not over
        }
    }
    //Basic movement down the screen
    private void CalculateMovement() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }
    //Function that runs once the enemy has died
    private void Death()
    {
        //Disable homing lasers from seeing the enemy
        _rb.isKinematic = true;
        FindObjectOfType<AudioManager>().Play("Boom");
        //play the enemy explosion animation
        _enemyanim.SetTrigger("isDead");
        //The enemy cant move while it is being destoryed
        _speed = 0;
        //Destory the enemy after a short delay
        Destroy(this.gameObject, 2.4f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Laser" && !_isDead)
        {
            _isDead = true;
            if (_player != null)
            {
                _player.AddScore(10);
            }
            Destroy(other.gameObject);
        }
        else if (other.tag == "Player" && !_isDead)
        {
            _isDead = true;
            if (_player != null)
            {
                _player.Damage();
            }
        }
        Death();
    }
}
