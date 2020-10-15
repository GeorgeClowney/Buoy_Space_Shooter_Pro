using UnityEngine;

//The Laser script is used by both the player and the enemies 
//It controls the Behaviour of the laserprefab

public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 8.0f, _rotateSpeed = 200f;
    // _isEnemyLaser controls if the laser belongs to the player or enemy
    private bool _isEnemyLaser;
    // We need the Rigidbody component for the homing laser behaviour
    private Rigidbody2D _rb;
    private Player _player;
    //_canSeeTarget is called only if a enemy is on the screen
    private bool _canSeeTarget;
    private void Awake()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("The Player is NULL(Laser)");
        }    
        _rb = GetComponent<Rigidbody2D>();
        if (_rb == null)
        {
             Debug.LogError("The Laser can't find it's Rigidbody2D Component");
        }
    }
    //FixedUpdate is needed for the homing laser to work correctly
    void FixedUpdate()
    {
        CheckForEnemies();
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
    }
    //check to see if at least one Enemy is currently on the screen
    private void CheckForEnemies()
    {
        if (GameObject.FindGameObjectsWithTag("Enemy").Length >= 1)
        {
            _canSeeTarget = true;
        }
        else
        {
            _canSeeTarget = false;
        }
    }
    private void MoveUp()
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
    //Make a laser a enemy laser
    //This controls how it is shot and changes the tag so that the laser can't hurt other enemies
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
        this.tag = "EnemyLaser";

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
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
