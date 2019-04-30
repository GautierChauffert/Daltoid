using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIn : MonoBehaviour
{

    public bool isIn = false;
    private Collision2D detect;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isIn = true;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isIn = false;
        }
    }

}
