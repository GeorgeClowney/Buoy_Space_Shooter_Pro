using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Basic Script that controls Explosion Behaviour
public class Explosion : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Boom");
        Destroy(this.gameObject, 3f);
    }
}
