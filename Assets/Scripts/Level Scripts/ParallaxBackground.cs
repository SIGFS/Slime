using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    private float length, startPos;
    public Camera cam;
    public float parallaxConstant;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = (cam.transform.position.x * (1 - parallaxConstant));

        float dist = (cam.transform.position.x * parallaxConstant);

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z);
    }

}
