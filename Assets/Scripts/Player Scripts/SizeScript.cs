using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [HideInInspector]public SizeScript instance;

    private float size;
    [SerializeField] private GameObject player;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        size = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        //temp size increase / decrease using keypress
        //change to different method in future
        if(Input.GetKeyDown(KeyCode.Equals) && size < 9.99) {
            SizeChangeUp();
        }
        if(Input.GetKeyDown(KeyCode.Minus) && size > 1.1) {
            SizeChangeDown();
        }
    }

    public void SizeChangeUp()
    {
        size += 1f;
        transform.position += new Vector3(0f, 0.15f, 0f);
        transform.localScale = new Vector3(size / 3, size / 3, 0f);
    }

    public void SizeChangeDown()
    {
        size -= 1f;
        transform.position -= new Vector3(0f, 0.15f, 0f);
        transform.localScale = new Vector3(size / 3, size / 3, 0f);
    }
}
