using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class SlimeBallScript : MonoBehaviour
{
    public GameObject player;

    private Rigidbody2D rgb;

    // Start is called before the first frame update
    void Start()
    {
        rgb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //stoping on hit
        if (collision.gameObject.tag == "Ground")
        {
            rgb.velocity = new Vector2(0, 0);
            rgb.isKinematic = true;

            //Debug.Log(transform.position);
        }
        
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //stoping on hit
        if (collision.gameObject.tag == "Ground")
        {
            hit = true;
            rgb.velocity = new Vector2(0,0);
            rgb.isKinematic = true;
        }
    }*/
}
