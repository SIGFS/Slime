using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [SerializeField] private float size;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement move;

    // Start is called before the first frame update
    void Start()
    {
        size = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        //temp size increase / decrease using keypress
        //change to different method in future
        if(Input.GetKeyDown(KeyCode.Equals) && size < 9.99) {
            size += 1f;
            transform.position += new Vector3(0f, 0.15f, 0f);
            transform.localScale = new Vector3(size/3, size/3, 0f);
            move.speed -= 0.35f;
            move.jumpHeight -= 0.35f;
        }
        if(Input.GetKeyDown(KeyCode.Minus) && size > 1.1) {
            size -= 1f;
            transform.position -= new Vector3(0f, 0.15f, 0f);
            transform.localScale = new Vector3(size/3, size/3, 0f);
            move.speed += 0.35f;
            move.jumpHeight += 0.35f;
        }
    }
}
