using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Transform colPosition = collision.gameObject.GetComponent<Transform>();
        Rigidbody2D colBody = collision.gameObject.GetComponent<Rigidbody2D>();
        float force = 25f;

        if (collision.gameObject.tag == "Player" | collision.gameObject.tag == "SlimeBall")
        {
            SizeScript playerSize = collision.gameObject.GetComponent<SizeScript>();
            //Check if player is above enemy, thrust upward
            if (colPosition.position.y > transform.position.y && colPosition.position.x > transform.position.x - 0.8f && colPosition.position.x < transform.position.x + 0.8f)
            {
                if (colBody.velocity.y < 0)
                    force -= colBody.velocity.y;

                colBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
            //Check if player is on wall
            if ((colPosition.position.x > transform.position.x | colPosition.position.x < transform.position.x)&& colPosition.position.y > transform.position.y - 0.8f && colPosition.position.y < transform.position.y + 0.8f)
            {
                if (colBody.velocity.y < 0)
                    force -= colBody.velocity.y;

                colBody.AddForce(Vector2.up * force, ForceMode2D.Impulse);
            }
        }
    }
}
