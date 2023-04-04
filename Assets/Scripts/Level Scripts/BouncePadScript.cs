using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePadScript : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Rigidbody2D colBody = collision.gameObject.GetComponent<Rigidbody2D>();

        if (collision.gameObject.tag == "Player")
        {
            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (!playerMovement.IsJumping)
            {
                float force = playerMovement.Data.jumpForce * 1.2f;
                playerMovement.BouncePad(force);
            }
                
        }
    }
}
