using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttackPointScript : MonoBehaviour
{
    [HideInInspector]public AttackPointScript instance;

    private float force = 2.5f;

    [Header("Instantiate")]
    public GameObject slimeBullet;
    public GameObject centerObject;

    private Transform centerPos;
    private float distance;
    private GameObject Player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Player = GameObject.FindGameObjectWithTag("Player");
        distance = Player.GetComponent<CircleCollider2D>().bounds.extents.x;
    }
    // Start is called before the first frame update
    void Start()
    {
        centerPos = centerObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        //direction calculation
        Vector2 CurPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 SlimePos = transform.position;
        Vector2 dir = CurPos - SlimePos;

        PointAtCursor(dir);
        if (Vector3.Distance(transform.position, centerPos.position) > distance)
        {
            distance = Player.GetComponent<CircleCollider2D>().bounds.extents.x * 0.5f;
            Vector3 directon = (transform.position - centerPos.position).normalized * distance;
            transform.position = centerPos.position + directon;
        }

        //firing
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(dir);
        }
    }

    //rotating to aim at the mouse
    void PointAtCursor(Vector2 dir)
    {
        transform.Translate(dir);
    }

    void Shoot(Vector2 dir)
    {
        if (!SizeScript.instance.isMinSize())
        {
            GameObject BallIns = Instantiate(slimeBullet, transform.position, Quaternion.identity);
            BallIns.GetComponent<Rigidbody2D>().velocity = dir * force;

            SizeScript.instance.SizeChangeDown();
        }
    }
}
