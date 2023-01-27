using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectPlayer : MonoBehaviour
{
    #region Variables
    [SerializeField] MapPoint startPoint = null;

    //speeds
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float teleportTime = 1f;

    //location of sprite
    [SerializeField] Transform playerSprite = null;

    //map points
    MapPoint[] allPoints;

    MapPoint prevPoint, currentPoint;

    //References
        //Add Animator

    SpriteRenderer spriteRenderer;

    //Player Movement
    float x, y;
    bool canMove = true;
    bool animationSet = false;
    int animating;
    Vector2 movement;
    #endregion

    #region Unity Methods
    void Awake()
    {
        allPoints = FindObjectsOfType<MapPoint>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Add animator
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.enabled = false;
        canMove = false;

        SetPlayerPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);

            if(Vector3.Distance(transform.position, currentPoint.transform.position) < 0.1f)
            {
                CheckMapPoint();
            }
            else
            {
                //Set Animation
            }
        }
    }

    void FixedUpdate()
    {
        GetMovement();
    }

    #endregion

    #region User Methods
    void AutoMove()
    {
        if(currentPoint.up != null && currentPoint.up != prevPoint)
        {
            SetNextPoint(currentPoint.up);
            animating = 1;
            animationSet = false;
        }
        else if(currentPoint.right != null && currentPoint.right != prevPoint)
        {
            SetNextPoint(currentPoint.right);
            animating = 1;
            animationSet = false;
        }
        else if(currentPoint.down != null && currentPoint.down != prevPoint)
        {
            SetNextPoint(currentPoint.down);
            animating = 1;
            animationSet = false;
        }
        else if(currentPoint.left != null && currentPoint.left != prevPoint)
        {
            SetNextPoint(currentPoint.left);
            animating = 2;
            animationSet = false;
        }
    }

    void CheckInput()
    {
        if(y > 0.5f)
        {
            if(currentPoint.up != null)
            {
                SetNextPoint(currentPoint.up);
                animating = 1;
                animationSet = false;
            }
        }
        if (x > 0.5f)
        {
            if (currentPoint.right != null)
            {
                SetNextPoint(currentPoint.right);
                animating = 1;
                animationSet = false;
            }
        }
        if (y < -0.5f)
        {
            if (currentPoint.down != null)
            {
                SetNextPoint(currentPoint.down);
                animating = 1;
                animationSet = false;
            }
        }
        if (x < -0.5f)
        {
            if (currentPoint.left != null)
            {
                SetNextPoint(currentPoint.left);
                animating = 2;
                animationSet = false;
            }
        }
    }

    void CheckMapPoint()
    {
        if (currentPoint.isWarpPoint && !currentPoint.hasWarped)
        {
            if (animating != 0)
            {
                animating = 0;
                SetAnimation();
            }

            //if Auto warp
                //Teleport Player
        }

        if(currentPoint.isCorner && currentPoint.isWarpPoint)
        {
            if(animating != 0)
            {
                animating = 0;
                SetAnimation();
            }

            CheckInput();
            //Select Level
        }

        if (currentPoint.isCorner)
        {
            AutoMove();
        }
        else
        {
            if(animating != 0)
            {
                animating = 0;
                SetAnimation();
            }

            CheckInput();
            //Select Level
        }

    }

    void SetAnimation()
    {
        //Set Animation State using Index
            //Animating == 0 -> Idle
            //Animating == 1 -> Walk
            //Animating == 2 -> Walk Left
    }

    void SetNextPoint(MapPoint nextPoint)
    {
        playerSprite.localPosition = Vector2.zero;

        prevPoint = currentPoint;

        currentPoint = nextPoint;
    }

    void SetPlayerPos()
    {
        //Check Save Data for current level
        //If player hasn't beaten the first level on launch
        transform.position = startPoint.transform.position;

        spriteRenderer.enabled = true;
        currentPoint = startPoint;
        prevPoint = currentPoint;

        canMove = true;

        //Else
            //Sort through all map points to find current level
            //Set player position to that level
    }

    public void GetMovement()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        x = movement.x;
        y = movement.y;
    }

    //Select Level Method
        //If click
            //if level
                //play start level sound
                //Set save data
                //Load level
            //if warp
                //teleport player

    //Teleport Player Method
        //update variables
        //for time
            //Set player opacity to nothing
        //move player
        //wait for time
        //for time
            //Set player opacity back
        //update variables


    #endregion

}
