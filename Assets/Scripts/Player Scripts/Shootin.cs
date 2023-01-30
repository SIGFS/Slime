using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shootin : MonoBehaviour
{
    public Vector2 dir;
    private float Force = 700;

    public GameObject slimeBullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //direction calculation
        Vector2 CurPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 SlimePos = transform.position;

        dir = CurPos - SlimePos;

        PointAtCursor();

        //firing
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
                
    }

    //rotating to aim at the mouse
    void PointAtCursor()
    {
        transform.right = dir;
    }

    void Shoot()
    {
        GameObject BallIns = Instantiate(slimeBullet, transform.position, Quaternion.identity);

        BallIns.GetComponent<Rigidbody2D>().AddForce(transform.right * Force);       
    }

}
