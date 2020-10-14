using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    private float _fireRate = 3.0f, _canFire = -1;
    private Player _player;
    private Animator _enemyanim;
    private bool _isDead;
    [SerializeField]
    private GameObject _laserPrefab;
    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Enemy script can't find the player");
        }
        _enemyanim = GetComponent<Animator>();
        if(_enemyanim == null) 
        {
            Debug.LogError("The Enemy can't find it's Animator component");
        }
    }
    void Update()
    {
        if (!_isDead)
        {
            CalculateMovement();
            if (Time.time > _canFire)
            {
                _fireRate = Random.Range(3f, 7f);
                _canFire = Time.time + _fireRate;
                GameObject eLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
                Laser[] lasers = eLaser.GetComponentsInChildren<Laser>();
                for (int i = 0; i < lasers.Length; i++)
                {
                    lasers[i].AssignEnemyLaser();
                }
            }
        }
    }

    private void CalculateMovement() {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            float randomX = Random.Range(-8f, 8f);
            transform.position = new Vector3(randomX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
   //     Debug.Log("Hit: " + other.transform.name);

        if(other.tag == "Laser" && !_isDead)
        {
            _isDead = true;
            if (_player != null)
            {
                _player.AddScore(10);
            }
             Destroy(other.gameObject);
            FindObjectOfType<AudioManager>().Play("Boom");
            _enemyanim.SetTrigger("isDead");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
        else if(other.tag == "Player" && !_isDead)
        {
            FindObjectOfType<AudioManager>().Play("Boom");
            _isDead = true;
            if(_player != null)
            {
                _player.Damage();
            }
            _enemyanim.SetTrigger("isDead");
            _speed = 0;
            Destroy(this.gameObject, 2.8f);
        }
    }
}
