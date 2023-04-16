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

    //location of sprite
    [SerializeField] Transform playerSprite = null;

    //map points
    MapPoint[] allPoints;

    MapPoint prevPoint, currentPoint;

    //References

    SpriteRenderer spriteRenderer;

    //Player Movement
    float x, y;
    bool canMove = true;
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

            if (Vector3.Distance(transform.position, currentPoint.transform.position) < 0.1f)
            {
                CheckMapPoint();
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
        if (currentPoint.up != null && currentPoint.up != prevPoint)
        {
            SetNextPoint(currentPoint.up);
        }
        else if (currentPoint.right != null && currentPoint.right != prevPoint)
        {
            SetNextPoint(currentPoint.right);
        }
        else if (currentPoint.down != null && currentPoint.down != prevPoint)
        {
            SetNextPoint(currentPoint.down);
        }
        else if (currentPoint.left != null && currentPoint.left != prevPoint)
        {
            SetNextPoint(currentPoint.left);
        }
    }

    void CheckInput()
    {
        if (y > 0.5f)
        {
            if (currentPoint.up != null)
            {
                SetNextPoint(currentPoint.up);
            }
        }
        if (x > 0.5f)
        {
            if (currentPoint.right != null)
            {
                SetNextPoint(currentPoint.right);
            }
        }
        if (y < -0.5f)
        {
            if (currentPoint.down != null)
            {
                SetNextPoint(currentPoint.down);
            }
        }
        if (x < -0.5f)
        {
            if (currentPoint.left != null)
            {
                SetNextPoint(currentPoint.left);
            }
        }
    }

    void CheckMapPoint()
    {

        if (currentPoint.isCorner)
        {
            AutoMove();
        }
        else
        {
            CheckInput();
            SelectLevel();
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
        if (DataManager.instance.gameData.currentLevelName == "")
        {
            transform.position = startPoint.transform.position;

            spriteRenderer.enabled = true;
            currentPoint = startPoint;
            prevPoint = currentPoint;

            canMove = true;
        }
        else
        {
            foreach (MapPoint point in allPoints)
            {
                if (point.isLevel)
                {
                    if (point.sceneToLoad == DataManager.instance.gameData.currentLevelName)
                    {
                        transform.position = point.transform.position;

                        spriteRenderer.enabled = true;
                        currentPoint = point;
                        prevPoint = currentPoint;

                        canMove = true;
                    }
                }
            }
        }
    }

    public void GetMovement()
    {
        movement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        x = movement.x;
        y = movement.y;
    }

    public void SelectLevel()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (currentPoint != null)
            {
                if (!currentPoint.isLocked && currentPoint.isLevel)
                {
                    LevelSelectAudio.instance.Play_StartLevelSound();

                    DataManager.instance.gameData.currentLevelName = currentPoint.sceneToLoad;
                    DataManager.instance.SaveGameData();

                    PlayerPrefs.SetInt("CurrentCheckpoint", 0);
                    SceneManager.LoadScene(currentPoint.sceneToLoad);
                }
            }
        }
    }

    #endregion

}
