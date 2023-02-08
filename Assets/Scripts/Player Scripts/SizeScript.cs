using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [HideInInspector]public static SizeScript instance { get; set; }

    private float _size;
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
        _size = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        //temp size increase / decrease using keypress
        //change to different method in future
        if(Input.GetKeyDown(KeyCode.Equals) && _size < 9.99) {
            SizeChangeUp();
        }
        if(Input.GetKeyDown(KeyCode.Minus) && _size > 1.1) {
            SizeChangeDown();
        }
    }

    public void SizeChangeUp()
    {
        _size += 1f;
        transform.position += new Vector3(0f, 0.15f, 0f);
        transform.localScale = new Vector3(_size / 3, _size / 3, 0f);
    }

    public void SizeChangeDown()
    {
        _size -= 1f;
        transform.position -= new Vector3(0f, 0.15f, 0f);
        transform.localScale = new Vector3(_size / 3, _size / 3, 0f);
    }

    public float Size
    {
        get { return _size; }
        set { _size = value; }
    }
}
