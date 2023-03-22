using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlimeBallScript : MonoBehaviour
{
    public GameObject bouncePrefab;

    private bool hitLeft, hitRight, hitUp, hitDown = false;
    private bool hitLeftTopCorner, hitLeftBottomCorner, hitRightTopCorner, hitRightBottomCorner = false;

    private GameObject newPad;

    private Tilemap tilemp;

    private void Start()
    {
        tilemp = GameObject.Find("Platform Tilemap").GetComponent<Tilemap>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //stoping on hit
        if (collision.gameObject.layer == 7) //If hits ground layer
        {
            Vector3Int playerPos = tilemp.WorldToCell(transform.position);
            DirectionCheck(playerPos);

            if (hitLeft)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x - 0.7f, transform.position.y), Quaternion.Euler(0f,0f,-90f));
            }
            if (hitRight)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x + 0.7f, transform.position.y), Quaternion.Euler(0f, 0f, 90f));
            }
            if (hitUp)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x, transform.position.y + 0.7f), Quaternion.Euler(0f, 0f, 180f));
            }
            if (hitDown)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x, transform.position.y - 0.6f), Quaternion.Euler(0f, 0f, 0f));
            }

            if (hitLeftTopCorner)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x - 0.7f, transform.position.y), Quaternion.Euler(0f, 0f, -135f));
            }
            if (hitLeftBottomCorner)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x - 0.4f, transform.position.y - 0.3f), Quaternion.Euler(0f, 0f, -45f));
            }
            if (hitRightTopCorner)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x + 0.7f, transform.position.y), Quaternion.Euler(0f, 0f, 135f));
            }
            if (hitRightBottomCorner)
            {
                Instantiate(bouncePrefab, new Vector2(transform.position.x + 0.4f, transform.position.y - 0.3f), Quaternion.Euler(0f, 0f, 45f));
            }
            gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
        }
        
    }

    void DirectionCheck(Vector3Int playerPos)
    {
        Vector3Int leftCheck = new Vector3Int(playerPos.x - 1, playerPos.y, playerPos.z);
        Vector3Int rightCheck = new Vector3Int(playerPos.x + 1, playerPos.y, playerPos.z);
        Vector3Int upCheck = new Vector3Int(playerPos.x, playerPos.y + 1, playerPos.z);
        Vector3Int downCheck = new Vector3Int(playerPos.x, playerPos.y - 1, playerPos.z);

        if(tilemp.GetTile(leftCheck) != null && (tilemp.GetTile(upCheck) == null && tilemp.GetTile(downCheck) == null))
        {
            hitLeft = true;
        }
        if (tilemp.GetTile(rightCheck) != null && (tilemp.GetTile(upCheck) == null && tilemp.GetTile(downCheck) == null))
        {
            hitRight = true;
        }
        if (tilemp.GetTile(upCheck) != null && (tilemp.GetTile(leftCheck) == null && tilemp.GetTile(rightCheck) == null))
        {
            hitUp = true;
        }
        if (tilemp.GetTile(downCheck) != null && (tilemp.GetTile(leftCheck) == null && tilemp.GetTile(rightCheck) == null))
        {
            hitDown = true;
        }
        if (tilemp.GetTile(leftCheck) != null && tilemp.GetTile(upCheck) != null)
        {
            hitLeftTopCorner = true;
        }
        if (tilemp.GetTile(leftCheck) != null && tilemp.GetTile(downCheck) != null)
        {
            hitLeftBottomCorner = true;
        }
        if (tilemp.GetTile(rightCheck) != null && tilemp.GetTile(upCheck) != null)
        {
            hitRightTopCorner = true;
        }
        if (tilemp.GetTile(rightCheck) != null && tilemp.GetTile(downCheck) != null)
        {
            hitRightBottomCorner = true;
        }


    }
}
