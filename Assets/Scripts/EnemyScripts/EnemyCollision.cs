using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public bool isSlimed;

    private bool applyForce = false;

    private Animator enemyAnim;
    private Collider2D myCollider;

    public void Awake()
    {
        isSlimed = false;
        enemyAnim = gameObject.GetComponent<Animator>();
        myCollider = gameObject.GetComponent<Collider2D>();
    }

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform colPosition = collision.gameObject.GetComponent<Transform>();
        Rigidbody2D colBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            float force = playerMovement.Data.jumpForce;

            SizeScript playerSize = collision.gameObject.GetComponent<SizeScript>();
            //Check if player is above enemy, thrust upward
            if (colPosition.position.y > transform.position.y /*&& colPosition.position.x > transform.position.x - myCollider.bounds.extents.x && colPosition.position.x < transform.position.x + myCollider.bounds.extents.x*/)
            {
                if (isSlimed)
                    force *= 1.5f;

                if (!playerMovement.IsJumping && !applyForce)
                {
                    applyForce = true;
                    playerMovement.BounceEnemy(force);
                }

                if (!isSlimed)
                    Destroy(gameObject.transform.parent.gameObject);
            } 
            //if player is not above, thrust backward
            else
            {
                Vector3 direction = Vector3.Normalize(colPosition.position - transform.position);
                colBody.AddForce(direction * force, ForceMode2D.Impulse);
                //If enemy is not slimed take damage
                if (!isSlimed)
                    playerSize.SizeChangeDown();
            }

            applyForce = false;
        }

        if (collision.gameObject.tag == "SlimeBall")
        {
            isSlimed = true;
            enemyAnim.SetTrigger("Slimed");

            gameObject.tag = "BouncePad";

            GetComponentInParent<EnemyScript>().speed /= 2;
        }
    }
}
