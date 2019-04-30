using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedwalk : MonoBehaviour
{
	[Header("Parameters")]
    [SerializeField, Range(1f, 50f)] private float speed = 20f;
    [SerializeField] private bool clockwise = true;

    void OnCollisionStay2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            if(clockwise)
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * speed * 1000f);
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * -speed * 1000f);
            }
            else
            {
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.right * (-speed * 1000f));
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * -speed * 1000f);
            }
        }
    }
}
