using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScript : MonoBehaviour
{
    [HideInInspector]public static SizeScript instance { get; set; }

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData Data;

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
    }

    public void SizeChangeUp()
    {
        if(!isMaxSize())
        {
            Data.size += 1;
            //Adjust position up to account for the change in size of object to prevent clipping through objects. Adjustment should be half of the size increase increment.
            transform.position += new Vector3(0f, 0.15f, 0f); 
            transform.localScale = new Vector3((float)Data.size * 0.3f, (float)Data.size * 0.3f, 0f);
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
}
