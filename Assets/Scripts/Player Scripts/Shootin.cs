using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shootin : MonoBehaviour
{
    public Rigidbody2D proj;

    public float forceMulti;
    public Vector2 angle;

    private bool charging = false;
    private float ChargeTime;
    public Image ChargeFill;

    public TextMeshProUGUI ChargeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FireForce(float force, Vector2 angle)
    {
        proj.AddForce(angle * force);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            charging = true;
            ChargeTime = 0;
        }
        else if(Input.GetKeyUp(KeyCode.Space))
        {
            charging = false;
            FireForce(ChargeTime * forceMulti, angle);
        }


        if(charging == true)
        {
            ChargeTime += Time.deltaTime;

        }

        if(ChargeText)
        {
            ChargeText.text = ChargeTime.ToString();
            ChargeFill.fillAmount = (float)ChargeTime / 10;
        }
    }
}
