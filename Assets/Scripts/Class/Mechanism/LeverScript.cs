using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LeverScript : ColorableEntity
{
	private Animator lev;
    private InputManager inputManager;

    public UnityEvent onLeverOn = new UnityEvent();
    [Range(0f, 1f)] public float threshold = 0.5f;


    protected override void Start()
    {
    	base.Start();
    	
        lev = GetComponentInChildren<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
    	ColorableEntity ent = collision.GetComponentInChildren<ColorableEntity>();

        if(collision.gameObject.CompareTag("Player") && Input.GetKeyDown("f"))
        {
            onLeverOn.Invoke();
            lev.Play("LeverOn");
        }
        else if (ent != null && ColorableEntity.ColorCompare(ent.color, this.color) > threshold)
        {
            onLeverOn.Invoke();
            lev.Play("LeverOn");
        }
    }
}


