using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//The powerup script controls the various powerups the player can collect
public class Powerup : MonoBehaviour
{
    //_speed controls how fast the powerups fall down the screen
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField]
    private int _powerupID;
    //ID for Powerups
    //0 = Triple Shot
    //1 = Speed 
    //2 = Shields
    //3 = Ammo
    //4 = Repair Kit
    //5 = Homing Shot
    void Update()
    {
        MoveDown();
    }
    //Function for Powerup Movement
    private void MoveDown()
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
            //Find and play the "PowerUp" Audio Clip
            FindObjectOfType<AudioManager>().Play("PowerUp");
            //Get a ref to the player from .other
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                //Switchcase that gives the Player script the functionality of the powerup that is collected
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
            //After the player collects the powerup destory it
            Destroy(this.gameObject);
        }
    }
}
