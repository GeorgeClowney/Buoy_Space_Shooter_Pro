using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed 
    //2 = Sheilds
    //3 = Ammo
    //4 = HP
    //5 = Homing Shot
    [SerializeField]
    private int _powerupID;
    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.y < -5f)
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            FindObjectOfType<AudioManager>().Play("PowerUp");
            Player player = other.GetComponent<Player>();
            if(player != null)
            {           
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.SheildActive();
                        break;
                    case 3:
                        player.AddAmmo();
                        break;
                    case 4:
                        player.AddLives();
                        break;
                    case 5:
                        player.AddHomingLaser();
                        break;
                    default:
                        Debug.Log("powerupID outside of switchcase");
                        break;


                }
            }
            Destroy(this.gameObject);
        }
    }
}
