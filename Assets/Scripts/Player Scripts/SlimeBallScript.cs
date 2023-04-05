using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlimeBallScript : MonoBehaviour
{
    #region Declarations
    public GameObject bouncePrefab;

    private int hitDirection = 5;

    private Tilemap tilemp;

    #endregion

    #region Unity Methods
    private void Start()
    {
        tilemp = GameObject.Find("Platform Tilemap").GetComponent<Tilemap>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //stoping on hit
        if (collision.gameObject.layer == 7) //If hits ground layer
        {
            Vector2 collisionPoint = collision.GetContact(0).point;
            DirectionCheck(collisionPoint);

            switch (hitDirection)
            {
                case 0/*Right*/:
                    {
                        Instantiate(bouncePrefab, new Vector2(collisionPoint.x, collisionPoint.y), Quaternion.Euler(0f, 0f, 90f));
                        Debug.Log("Spawn Right");
                        break;
                    }
                case 1/*Down*/:
                    {
                        Instantiate(bouncePrefab, new Vector2(collisionPoint.x, collisionPoint.y), Quaternion.Euler(0f, 0f, 0f));
                        Debug.Log("Spawn Down");
                        break;
                    }
                case 2/*Left*/:
                    {
                        Instantiate(bouncePrefab, new Vector2(collisionPoint.x, collisionPoint.y), Quaternion.Euler(0f, 0f, -90f));
                        Debug.Log("Spawn Left");
                        break;
                    }
            }
            gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Check Spawning Orientation
    void DirectionCheck(Vector2 point)
    {
        //Raycast collision point
        if(Physics2D.Raycast(point, Vector2.right, 0.1f))
        {
            //Right
            hitDirection = 0;
        }
        if (Physics2D.Raycast(point, Vector2.down, 0.1f))
        {
            //Down
            hitDirection = 1;
        }
        if (Physics2D.Raycast(point, Vector2.left, 0.1f))
        {
            //Left
            hitDirection = 2;
        }

    }
    #endregion
}
