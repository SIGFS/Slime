using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class SlimeBallScript : MonoBehaviour
{
    public GameObject player;

    private Rigidbody2D rgb;

    bool hit = false;
    // Start is called before the first frame update
    void Start()
    {
        

        rgb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (hit == false)
        {
            TrackCursor();
        }
    }

    //following the mouse
    void TrackCursor()
    {
        Vector2 dir = rgb.velocity;



        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //stoping on hit
        if (collision.gameObject.tag == "Ground")
        {
            hit = true;
            rgb.velocity = new Vector2(0, 0);
            rgb.isKinematic = true;

            Debug.Log(transform.position);
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
