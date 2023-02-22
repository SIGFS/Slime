using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    public bool isSlimed;

    private Animator enemyAnim;

    public void Awake()
    {
        isSlimed = false;
        enemyAnim = gameObject.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform colPosition = collision.gameObject.GetComponent<Transform>();
        Rigidbody2D colBody = collision.gameObject.GetComponent<Rigidbody2D>();
        float force = 25f;

        if (collision.gameObject.tag == "Player")
        {
            SizeScript playerSize = collision.gameObject.GetComponent<SizeScript>();
            //Check if player is above enemy, thrust upward
            if (colPosition.position.y > transform.position.y && colPosition.position.x > transform.position.x - 0.8f && colPosition.position.x < transform.position.x + 0.8f)
            {
                if (colBody.velocity.y < 0)
                    force -= colBody.velocity.y;
                if (isSlimed)
                    force *= 1.5f;

                colBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);

                if (!isSlimed)
                    Destroy(gameObject.transform.parent.gameObject);
            } 
            //if player is not above, thrust backward and reduce size
            else
            {
                Vector3 direction = Vector3.Normalize(colPosition.position - transform.position);
                colBody.AddForce(direction * force, ForceMode2D.Impulse);
                playerSize.SizeChangeDown();
            }
        }

        if (collision.gameObject.tag == "SlimeBall")
        {
            isSlimed = true;
            enemyAnim.Play("SlimedMove");

            gameObject.tag = "BouncePad";

            GetComponentInParent<EnemyScript>().speed /= 2;
        }
    }
}
