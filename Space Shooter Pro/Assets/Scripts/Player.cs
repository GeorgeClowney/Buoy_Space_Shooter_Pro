using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The Player script controls everything revoling the player
//Is currently being updated
public class Player : MonoBehaviour
{
    //Player stats
    [SerializeField]
    private int _lives = 3, _score, _ammo = 15, _shields;
    public int homingShots;
    [SerializeField]
    private float _speed = 3.5f, _fireRate = 0.5f, _canFire = -1f, _thusterEnergy = 15f;
    private float _startingSpeed, _speedBoosted;
    [SerializeField]
    private GameObject _laserPrefab, _tripleShotPrefab, _sheildPrefab, _leftEnginePrefab, _rightEnginePrefab;
    [SerializeField]
    private bool _tripleShot, _speedBoost, _thrustersEngaged, _leftHit, _rightHit;
    private UIManager _uiManager;
    private SpawnManager _spawnManager;
    private CameraShake _cameraShake;
    void Start()
    {
        _startingSpeed = _speed;
        _speedBoosted = _speed + 5f;
        transform.position = new Vector3(0, 0, 0);
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL");
        }
        _uiManager = GameObject.Find("UI").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("The UI is NULL");
        }
        _cameraShake = GameObject.Find("CameraHolder").GetComponentInChildren<CameraShake>();
        if (_spawnManager == null)
        {
            Debug.LogError("The Player can't find the CameraShake script inside the CameraHolder Prefab");
        }
    }
    void Update()
    {if (_shields >= 1)
        {
            _uiManager.UpdateShield(_shields);
            _sheildPrefab.SetActive(true);
        } else 
        {
            _uiManager.UpdateShield(_shields);
            _sheildPrefab.SetActive(false);
        }
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            if (homingShots >= 1)
            {
                homingShots--;
                _uiManager.UpdateHomingShots(homingShots);
            }
            Shooting();
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            if(_thusterEnergy > 3f)
            {
                print("Powering Thrusters...");
                _thrustersEngaged = true;
                _thusterEnergy = _thusterEnergy - .09f;
                _uiManager.SetEnergy(_thusterEnergy);
                if(_thusterEnergy < 2f)
                {
                    _thrustersEngaged = false;
                }
            } 
            else
            {
                print("not enough energy");
            }
        }
        else
        {
            _uiManager.SetEnergy(_thusterEnergy);
            _thrustersEngaged = false;
            _thusterEnergy = _thusterEnergy + .01f;
            if(_thusterEnergy>=15)
            {
                _thusterEnergy = 15;
            }
        }
        if (_speedBoost || _thrustersEngaged)
        {
            _speed = _speedBoosted;
        }
        else
        {
            _speed = _startingSpeed;
        }
    }

    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

private void Shooting()
    {
        if (_ammo > 0)
        {         
            _ammo--;
            _uiManager.SetAmmo(_ammo);
            _canFire = Time.time + _fireRate;
            FindObjectOfType<AudioManager>().Play("Laser");
            if (!_tripleShot)
            {
                Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
            }
            else
            {
                Instantiate(_tripleShotPrefab, transform.position, Quaternion.identity);
            }
        }
    }
    private void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        //Basic movement
        transform.Translate(direction * _speed * Time.deltaTime);
        //Clamp locks the player y to the middle and bottom of the screen
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);
        //Warp the player from the left and right of the screen
        if (transform.position.x > 11.3f)
        {
            transform.position = new Vector3(-11.3f, transform.position.y, 0);
        }
        else if (transform.position.x < -11.3f)
        {
            transform.position = new Vector3(11.3f, transform.position.y, 0);
        }
    }
    public void Damage()
    {
        StartCoroutine(_cameraShake.Shake(.15f, .5f));
        if (_shields >= 1)
        {
            _shields--;
            _uiManager.UpdateShield(_shields);
        }
        else
        {
            _lives--;
            _uiManager.UpdateLives(_lives);
            if (!_leftHit && !_rightHit)
            {
                int randomHit = Random.Range(0, 2);
                switch (randomHit)
                {
                    case 0:
                        _leftEnginePrefab.SetActive(true);
                        _leftHit = true;
                        break;
                    case 1:
                        _rightEnginePrefab.SetActive(true);
                        _rightHit = true;
                        break;
                    default:
                        Debug.LogError("Issue with randomHit");
                        break;
                }
            }
            else if (_rightHit && !_leftHit)
            {
                _leftEnginePrefab.SetActive(true);
                _leftHit = true;
            }
            else
            {
                _rightEnginePrefab.SetActive(true);
                _rightHit = true;
            }
            if (_lives <= 0)
            {
                _spawnManager.OnPlayerDeath();
                Destroy(this.gameObject);
            }
        }

    }
    //Powerups
    public void SheildActive()
    {
        _shields = 3;
        _uiManager.UpdateShield(_shields);
    }
    public void TripleShotActive()
    {
        _tripleShot = true;
        StartCoroutine(PowerDownRoutine());
    }
    public void SpeedBoostActive()
    {
        _speedBoost = true;
        StartCoroutine(PowerDownRoutine());
    }
    public void AddAmmo()
    {
        _ammo = 15;
        _uiManager.SetAmmo(_ammo);
    }
    public void AddLives()
    {
        if(_lives<=2)
        {
            _lives++;
            _uiManager.UpdateLives(_lives);
            Repair();
        }    
    }
    private void Repair()
    {
        //print("Repairing");
        if(!_leftHit && _rightHit)
        {
            _rightHit = false;
            _rightEnginePrefab.SetActive(false);
        }
        else if (_leftHit && !_rightHit)
        {
            _leftHit = false;
            _leftEnginePrefab.SetActive(false);
        }
        else if(_leftHit && _rightHit)
        {
            _leftHit = false;
            _leftEnginePrefab.SetActive(false);
        }
      
    }
    public void AddHomingLaser()
    {
        homingShots += 5;
        _uiManager.UpdateHomingShots(homingShots);
    }
    IEnumerator PowerDownRoutine()
    {
        yield return new WaitForSeconds(5.0f);
        _tripleShot = false;
        _speedBoost = false;
    }

}
