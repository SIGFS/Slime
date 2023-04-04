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

    private bool hitLeft, hitRight, hitUp, hitDown = false;

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
            Vector3Int collisionPos = tilemp.WorldToCell(transform.position);
            DirectionCheck(collisionPos);

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
            gameObject.SetActive(false);
        }
        if(collision.gameObject.tag == "Enemy")
        {
            gameObject.SetActive(false);
        }
    }
    #endregion

    #region Check Spawning Orientation
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
    }
    #endregion
}
