using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f, _rotateSpeed = 200f;
    private bool _isEnemyLaser;
    private Rigidbody2D _rb;
    private Player _player;
    private bool _canSeeTarget;
    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();

    }
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); 
        if(_rb == null)
        {
            Debug.LogError("The Laser can't find it's Rigidbody2D");
        }
    }
    void FixedUpdate()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= 1)
        {
            _canSeeTarget = true;
        } else
        {
            _canSeeTarget = false;
        }
            if (!_isEnemyLaser)
        {
            if (_player.homingShots >= 1 && _canSeeTarget)
            {
                HomingLaser();
            }
            else
            {
                MoveUp();
            }
        }
        else
        {
            MoveDown();
        }

        void MoveUp()
        {
            transform.Translate(Vector3.up * _speed * Time.deltaTime);

            if (transform.position.y > 8f)
            {
                if (transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
        void HomingLaser()
        {
                Transform _enemy;
                _enemy = GameObject.FindGameObjectWithTag("Enemy").transform;
                Vector2 direction = (Vector2)_enemy.position - _rb.position;
                direction.Normalize();
                float _rotateAmount = Vector3.Cross(direction, transform.up).z;
                _rb.angularVelocity = -_rotateAmount * _rotateSpeed;
                _rb.velocity = transform.up * _speed;
            }
        }



        void MoveDown()
        {
            transform.Translate(Vector3.down * _speed * Time.deltaTime);

            if (transform.position.y < -11f)
            {
                if (transform.parent != null)
                {
                    Destroy(this.transform.parent.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    
    
   public void AssignEnemyLaser()
   {
        _isEnemyLaser = true;
   }
    private void OnTriggerEnter2D(Collider2D other)
    {
      //  Debug.Log("Hit: " + other.transform.name);

        if (other.tag == "Player" && _isEnemyLaser)
        {
            Player player = other.GetComponent<Player>();

            if (player != null)
            {
                player.Damage();
                Destroy(this.gameObject);
            }
        }
    }
}
