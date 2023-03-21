using System.Collections;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SlimeBallScript : MonoBehaviour
{
    public GameObject bouncePrefab;

    private float raycastDistance = 0.005f;
    private int groundLayer = 7;
    private bool hitLeft, hitRight, hitUp, hitDown = false;

    private Vector2 leftTilePos, rightTilePos, upTilePos, downTilePos;

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
                Instantiate(bouncePrefab, new Vector2(transform.position.x, transform.position.y - 0.7f), Quaternion.Euler(0f, 0f, 0f));
            }
            this.gameObject.SetActive(false);

        }
        
    }

    void DirectionCheck(Vector3Int playerPos)
    {
        Vector3Int leftCheck = new Vector3Int(playerPos.x - 1, playerPos.y, playerPos.z);
        Vector3Int rightCheck = new Vector3Int(playerPos.x + 1, playerPos.y, playerPos.z);
        Vector3Int upCheck = new Vector3Int(playerPos.x, playerPos.y + 1, playerPos.z);
        Vector3Int downCheck = new Vector3Int(playerPos.x, playerPos.y - 1, playerPos.z);

        if(tilemp.GetTile(leftCheck) != null)
        {
            hitLeft = true;
        }
        if (tilemp.GetTile(rightCheck) != null)
        {
            hitRight = true;
        }
        if (tilemp.GetTile(upCheck) != null)
        {
            hitUp = true;
        }
        if (tilemp.GetTile(downCheck) != null)
        {
            hitDown = true;
        }


    }
}
