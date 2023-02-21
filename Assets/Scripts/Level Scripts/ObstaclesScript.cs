using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesScript : MonoBehaviour
{
    private Rigidbody2D myBody;

    // Start is called before the first frame update
    void Awake()
    {
        myBody = this.GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("AdamScene");
        }
    }
}
