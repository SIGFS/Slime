using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;

    public GameObject enemy;
    public Transform leftGate;
    public Transform rightGate;

    private SpriteRenderer enemySprite;
    private Collider2D enemyBox;
    private bool isSlimed;

    private void Awake()
    {
        enemySprite = enemy.GetComponent<SpriteRenderer>();
        enemyBox = enemy.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if(enemy.transform.position.x <= leftGate.position.x)
        {
            if (!enemySprite.flipX)
            {
                enemyBox.offset = new Vector2(enemyBox.offset.x * -1, enemyBox.offset.y);
                enemySprite.flipX = true;
            }
            enemy.transform.Translate(Vector3.right * speed * Time.deltaTime);
        } 
        else if (enemySprite.flipX && enemy.transform.position.x > leftGate.position.x && enemy.transform.position.x < rightGate.position.x)
        {
            enemy.transform.Translate(Vector3.right * speed * Time.deltaTime);
        } 
        else if (!enemySprite.flipX && enemy.transform.position.x > leftGate.position.x && enemy.transform.position.x < rightGate.position.x)
        {
            enemy.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        else if (enemy.transform.position.x >= rightGate.position.x)
        {
            if (enemySprite.flipX)
            {
                enemyBox.offset = new Vector2(enemyBox.offset.x * -1, enemyBox.offset.y);
                enemySprite.flipX = false;
            }
            enemy.transform.Translate(Vector3.left * speed * Time.deltaTime);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform colPosition = collision.gameObject.GetComponent<Transform>();
        
        if(collision.gameObject.tag == "Player")
        {
            if (colPosition.position.y > transform.position.y && colPosition.position.x > transform.position.x - 0.8f && colPosition.position.x < transform.position.x + 0.8f)
            {

            }
        }
    }
}
