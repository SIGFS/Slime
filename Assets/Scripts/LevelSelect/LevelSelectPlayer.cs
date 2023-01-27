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
    Animator anim;

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
        anim = GetComponentInChildren<Animator>();
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
                if (!animationSet)
                    SetAnimation();
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

            if (currentPoint.autoWarp && !currentPoint.isLocked)
                StartCoroutine(TeleportPlayer(teleportTime));
        }

        if(currentPoint.isCorner && currentPoint.isWarpPoint)
        {
            if(animating != 0)
            {
                animating = 0;
                SetAnimation();
            }

            CheckInput();
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

        }

    }

    void SetAnimation()
    {
        animationSet = true;

        switch (animating)
        {
            case 0:
                anim.Play("Idle");
                break;
            case 1:
                anim.Play("Walk");
                break;
            case 2:
                anim.Play("WalkLeft");
                break;
        }
    }

    void SetNextPoint(MapPoint nextPoint)
    {
        playerSprite.localPosition = Vector2.zero;

        prevPoint = currentPoint;

        currentPoint = nextPoint;
    }

    void SetPlayerPos()
    {
        transform.position = startPoint.transform.position;

        spriteRenderer.enabled = true;
        currentPoint = startPoint;
        prevPoint = currentPoint;

        canMove = true;
    }

    public void GetMovement()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        x = movement.x;
        y = movement.y;
    }

    IEnumerator TeleportPlayer(float time)
    {
        currentPoint.hasWarped = true;
        canMove = false;

        for(float t = 0.0f; t < 1.0f; t+= Time.deltaTime / time)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(1, 0, t));
            spriteRenderer.color = newColor;
            yield return null;
        }

        transform.position = currentPoint.warpPoint.transform.position;

        yield return new WaitForSeconds(time);

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(0, 1, t));
            spriteRenderer.color = newColor;
            yield return null;
        }

        currentPoint = currentPoint.warpPoint;

        currentPoint.hasWarped = true;

        canMove = true;
    }

    #endregion

}
