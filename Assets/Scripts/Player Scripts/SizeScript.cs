using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [SerializeField] private float size = 1f;
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerMovement move;

    // Start is called before the first frame update
    void Start()
    {
        
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
            move.maxSpeed -= 0.2f;
        }
        if(Input.GetKeyDown(KeyCode.Minus)) {
            size -= 0.1f;
            transform.position -= new Vector3(0f, 0.05f, 0f);
            transform.localScale = new Vector3(size, size, size);
            move.maxSpeed += 0.2f;
        }
    }
}
