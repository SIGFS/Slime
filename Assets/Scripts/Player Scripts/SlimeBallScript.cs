using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlimeBallScript : MonoBehaviour
{
    #region Declarations
    [SerializeField] private GameObject bouncePrefab;

    private int hitDirection = 5;
    [SerializeField] private bool isDennis;

    [Header("Slime Ball Colliders")]
    [SerializeField] private GameObject leftCheck;
    [SerializeField] private GameObject rightCheck, downCheck;

    private Tilemap tilemp;
    private Vector3 cellPosition;
    private bool hit;

    #endregion

    #region Unity Methods
    private void Start()
    {
        tilemp = GameObject.Find("Platform Tilemap").GetComponent<Tilemap>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isDennis)
        {
            //stoping on hit
            if (collision.gameObject.layer == 7) //If hits ground layer
            {
                if (collision.gameObject.tag == "Object")
                {
                    gameObject.SetActive(false);
                    return;
                }
                if (collision.gameObject.tag == "Button")
                {
                    gameObject.SetActive(false);
                    collision.gameObject.GetComponent<ButtonScript>().PressButton();
                    return;
                }
                DirectionCheck();

                //Rotation default is 0, 0, 0
                Vector3 rotation = new Vector3(0f, 0f, 0f);
                Vector3 position = new Vector3(cellPosition.x, cellPosition.y, 0f);

                //Set rotation
                switch (hitDirection)
                {
                    case 0/*Down*/:
                        {
                            position = new Vector3(cellPosition.x, cellPosition.y + 0.16f, 0f);
                            Debug.Log("Spawn Down");
                            break;
                        }
                    case 1/*Right*/:
                        {
                            position = new Vector3(cellPosition.x + 0.16f, cellPosition.y, 0f);
                            Debug.Log("Spawn Right");
                            rotation = new Vector3(0f, 0f, 90f);
                            break;
                        }
                    case 2/*Left*/:
                        {
                            position = new Vector3(cellPosition.x - 0.16f, cellPosition.y, 0f);
                            rotation = new Vector3(0f, 0f, -90f);
                            Debug.Log("Spawn Left");
                            break;
                        }
                }
                //Instantiate new bouncepad
                if (hit)
                {
                    GameObject newBouncePad = Instantiate(bouncePrefab, position, Quaternion.Euler(rotation));

                    //If collision is with a moving platform, set bouncepad as a child
                    if (collision.gameObject.tag == "Moving Platform")
                        newBouncePad.transform.parent = collision.transform;
                }

                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "DeathZone")
        {
            gameObject.GetComponent<Rigidbody2D>().velocity.Scale(new Vector2(0f, 0.1f));
        }
    }
    #endregion

    #region Check Spawning Orientation
    void DirectionCheck()
    {
        //Raycast collision point
        if (Physics2D.Raycast(downCheck.transform.position, Vector2.down, 0.1f))
        {
            //Down
            hit = true;
            hitDirection = 0;
            cellPosition = tilemp.CellToWorld(tilemp.WorldToCell(new Vector3(downCheck.transform.position.x, downCheck.transform.position.y - 0.16f, 0f)));
            return;
        }
        if (Physics2D.Raycast(rightCheck.transform.position, Vector2.right, 0.1f))
        {
            //Right
            hit = true;
            hitDirection = 1;
            cellPosition = tilemp.CellToWorld(tilemp.WorldToCell(new Vector3(rightCheck.transform.position.x + 0.16f, rightCheck.transform.position.y, 0f)));
            return;
        }
        if (Physics2D.Raycast(leftCheck.transform.position, Vector2.left, 0.1f))
        {
            //Left
            hit = true;
            hitDirection = 2;
            cellPosition = tilemp.CellToWorld(tilemp.WorldToCell(new Vector3(leftCheck.transform.position.x - 0.16f, downCheck.transform.position.y, 0f)));
            return;
        }

    }
    #endregion
}
