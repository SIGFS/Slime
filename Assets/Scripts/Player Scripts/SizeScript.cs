using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [SerializeField] private float size = 1f;
    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        //temp size increase / decrease using keypress
        //change to different method in future
        if(Input.GetKeyDown(KeyCode.Equals)) {
            size += 0.1f;
            transform.position += new Vector3(0f, 0.05f, 0f);
            transform.localScale = new Vector3(size, size, size);
            player.GetComponent<PlayerMovement>().maxSpeed -= 0.2f;
        }
        if(Input.GetKeyDown(KeyCode.Minus)) {
            size -= 0.1f;
            transform.position -= new Vector3(0f, 0.05f, 0f);
            transform.localScale = new Vector3(size, size, size);
            player.GetComponent<PlayerMovement>().maxSpeed += 0.2f;
        }
    }
}
