using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformScript : MonoBehaviour
{
    public float waitTimeX;
    public float waitTimeY;
    public int cellsUp;
    public int cellsDown;
    public int cellsLeft;
    public int cellsRight;
    public bool startRight;
    public bool startDown;

    private Vector3 start;
    private bool isMovingRight;
    private bool isMovingDown;
    public float speed;
    private Grid worldgrid;
    private float cellX;
    private float cellY;
    private float xTime = 0;
    private float yTime = 0;
    private Vector3 offset;
    private List<GameObject> targets;

    private void Awake()
    {
        worldgrid = GameObject.FindGameObjectWithTag("World").GetComponent<Grid>();
        cellX = worldgrid.cellSize.x;
        cellY = worldgrid.cellSize.y;
    }

    private void Start()
    {
        isMovingRight = startRight;
        isMovingDown = startDown;
        start = transform.position;
        target = null;
    }

    private void Update()
    {
        if(cellsLeft != 0 || cellsRight != 0)
            MoveX();
        if(cellsUp != 0 || cellsDown != 0)
            MoveY();
    }

    private void MoveX()
    {
        if (xTime <= 0)
        {
            if (transform.position.x > start.x + (cellX * cellsRight) * transform.localScale.x && isMovingRight)
            {
                isMovingRight = false;
                xTime = waitTimeX;
            }

            if (transform.position.x < start.x - (cellX * cellsLeft) * transform.localScale.x && !isMovingRight)
            {
                isMovingRight = true;
                xTime = waitTimeX;
            }

            if (isMovingRight)
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
        }
        else
        {
            xTime -= Time.deltaTime;
        }
    }

    private void MoveY()
    {
        if (yTime <= 0)
        {
            if (transform.position.y > start.y + (cellY * cellsUp) * transform.localScale.y && !isMovingDown)
            {
                isMovingDown = true;
                yTime = waitTimeY;
            }

            if (transform.position.y < start.y - (cellY * cellsDown) * transform.localScale.y && isMovingDown)
            {
                isMovingDown = false;
                yTime = waitTimeY;
            }

            if (isMovingDown)
            {
                transform.Translate(Vector2.down * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.up * speed * Time.deltaTime);
            }
        } else
        {
            yTime -= Time.deltaTime;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        targets.Add(col.gameObject);
        offset = targets[0].transform.position - transform.position;
    }
    void OnTriggerExit2D(Collider2D col)
    {
        targets.Remove(col.gameObject);
    }
    void LateUpdate()
    {
        if (targets != null)
        {
            foreach (GameObject t in targets){
                t.transform.position = transform.position + offset;
            }
        }
    }
}
