using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [HideInInspector]public static SizeScript instance { get; set; }

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData Data;
    [SerializeField] private GameObject attackPoint;

    private float wellDelay;
    private float wellDelayMax = 1f;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        Data.size = 5;
        dataChange();
    }

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3((float)Data.size * 0.3f, (float)Data.size * 0.3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        //temp size increase / decrease using keypress
        //commented out in in case of future need
        /*if(Input.GetKeyDown(KeyCode.Equals)) {
            SizeChangeUp();
        }
        if(Input.GetKeyDown(KeyCode.Minus)) {
            SizeChangeDown();
        }*/
        wellDelay += Time.deltaTime;
    }

    public void SizeChangeUp()
    {
        if(!isMaxSize())
        {
            Data.size += 1;
            //Adjust position up to account for the change in size of object to prevent clipping through objects. Adjustment should be half of the size increase increment.
            transform.position += new Vector3(0f, 0.15f, 0f); 
            transform.localScale = new Vector3((float)Data.size * 0.3f, (float)Data.size * 0.3f, 0f);
            dataChange();

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            attackPoint.transform.localScale = (scale / 3);
        }

    }

    public void SizeChangeDown()
    {
        if(!isMinSize())
        {
            Data.size -= 1;
            //Adjust position down to put player back on floor. Adjustment should be half of the size decrease increment.
            transform.position -= new Vector3(0f, 0.15f, 0f); 
            transform.localScale = new Vector3((float)Data.size * 0.3f, (float)Data.size * 0.3f, 0f);
            dataChange();

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            attackPoint.transform.localScale = (scale / 3);
        }
    }

    public int getSize() {
        return Data.size;
    }

    public void setSize(int size) {
        Data.size = size;
    }

    public bool isMaxSize() {
        if(Data.size == 10) return true;
        return false;
    }

    public bool isMinSize() {
        if(Data.size == 1) return true;
        return false;
    }

    public void dataChange()
    {
        //formula

        //Data.jumpHeight = -.7777777f * getSize() + 7.7777777f;
        Data.jumpHeight = -.55f * getSize() + 5.5f;
        Data.jumpTimeToApex = -.05f * getSize() + .55f;
        Data.runMaxSpeed = -1.1111f * getSize() + 17.111111f;
        Data.jumpForce = Mathf.Abs(Data.gravityStrength) * (-.05f * getSize() + .55f);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Spike")
        {
            SizeChangeDown();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Well" && wellDelay >= wellDelayMax)
        {
            wellDelay = 0f;
            SizeChangeUp();
        }
    }
}
