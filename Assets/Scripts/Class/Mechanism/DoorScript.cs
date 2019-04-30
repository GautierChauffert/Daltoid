using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    private Animator anim;
    private Collider2D coll;
    private bool isOpened = false;
    
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        coll = GetComponent<Collider2D>();
    }

    public void OpenDoor()
    {
        if(isOpened == false)
        {
            coll.enabled = false;
            isOpened = true;
            AudioManager.instance.PlaySound("Door");
            anim.Play("NewDoorOpening");
        }
    }
}
