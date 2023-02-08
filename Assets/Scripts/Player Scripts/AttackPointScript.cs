using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AttackPointScript : MonoBehaviour
{
    public AttackPointScript instance;

    private float force = 2.5f;

    public GameObject slimeBullet;

    private Transform centerPos;

    [SerializeField] private float distance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        centerPos = GameObject.FindGameObjectWithTag("Player").transform;
        distance = this.GetComponentInParent<CircleCollider2D>().bounds.extents.x;
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
        GameObject BallIns = Instantiate(slimeBullet, transform.position, Quaternion.identity);

        BallIns.GetComponent<Rigidbody2D>().velocity = dir * force;       
    }
    public void ChangeSize()
    {
        distance = this.GetComponentInParent<CircleCollider2D>().bounds.extents.x;
        Debug.Log(distance);
    }

}
