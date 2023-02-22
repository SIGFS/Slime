using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float speed;

    public bool notGated;

    public GameObject enemy;
    public Transform leftGate;
    public Transform rightGate;

    public Transform groundCheck;
    public Transform wallCheck;
    public LayerMask ground;

    private SpriteRenderer enemySprite;
    private Collider2D enemyBox;
    public bool hitWall;
    public bool hitEdge;

    private void Awake()
    {
        enemySprite = enemy.GetComponent<SpriteRenderer>();
        enemyBox = enemy.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (notGated)
        {
            checkGround();
            checkWall();
        }
        
        if(enemy != null)
            Move();
    }

    private void Move()
    {
        if (!notGated)
        {
            //Move back and forth between gates
            if (enemy.transform.position.x <= leftGate.position.x)
            {
                if (!enemySprite.flipX)
                {
                    enemyBox.offset = new Vector2(enemyBox.offset.x * -1, enemyBox.offset.y);
                    enemySprite.flipX = true;
                }
                MoveRight();
            }
            else if (enemySprite.flipX && enemy.transform.position.x > leftGate.position.x && enemy.transform.position.x < rightGate.position.x)
            {
                MoveRight();
            }
            else if (!enemySprite.flipX && enemy.transform.position.x > leftGate.position.x && enemy.transform.position.x < rightGate.position.x)
            {
                MoveLeft();
            }
            else if (enemy.transform.position.x >= rightGate.position.x)
            {
                if (enemySprite.flipX)
                {
                    enemyBox.offset = new Vector2(enemyBox.offset.x * -1, enemyBox.offset.y);
                    enemySprite.flipX = false;
                }
                MoveLeft();
            }
        }
        else
        {
            //Move back and forth between walls and edges
            if ((hitEdge || hitWall) && !enemySprite.flipX)
            {
                enemyBox.offset = new Vector2(enemyBox.offset.x * -1, enemyBox.offset.y);
                enemySprite.flipX = true;

                groundCheck.localPosition = new Vector3(-groundCheck.localPosition.x, groundCheck.localPosition.y, groundCheck.localPosition.z);
                wallCheck.localPosition = new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, wallCheck.localPosition.z);
                MoveLeft();
            }
            else if (!enemySprite.flipX)
            {
                MoveLeft();
            }
            else if ((hitEdge || hitWall) && enemySprite.flipX)
            {
                enemyBox.offset = new Vector2(enemyBox.offset.x * -1, enemyBox.offset.y);
                enemySprite.flipX = false;

                groundCheck.localPosition = new Vector3(-groundCheck.localPosition.x, groundCheck.localPosition.y, groundCheck.localPosition.z);
                wallCheck.localPosition = new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, wallCheck.localPosition.z);
                MoveRight();
            }
            else if (enemySprite.flipX)
            {
                MoveRight();
            }
        }

    }

    private void checkGround()
    {
        hitEdge = !Physics2D.Raycast(groundCheck.position, Vector2.down, 0.1f, ground);
    }

    private void checkWall()
    {
        Vector3 direction;
        if (enemySprite.flipX)
            direction = Vector3.right;
        else
            direction = Vector3.left;
        
        hitWall = Physics2D.Raycast(wallCheck.position, direction, 0.1f, ground);
    }

    private void MoveRight()
    {
        enemy.transform.Translate(Vector3.right * speed * Time.deltaTime);
    }

    private void MoveLeft()
    {
        enemy.transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
    
}
