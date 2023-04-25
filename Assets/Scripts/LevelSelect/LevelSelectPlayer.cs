using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelSelectPlayer : MonoBehaviour
{
    #region Variables
    [SerializeField] MapPoint buildingLevel = null;
    [SerializeField] MapPoint desertLevel = null;
    [SerializeField] MapPoint forestLevel = null;

    //speeds
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float teleportTime = 1f;

    //location of sprite
    [SerializeField] Transform playerSprite = null;

    //map points

    MapPoint prevPoint, currentPoint;

    //References
    Animator anim;

    [SerializeField] SpriteRenderer spriteRenderer;

    //Player Movement
    float x, y;
    bool canMove = true;
    Vector2 movement;
    #endregion

    #region Unity Methods
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
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
            else
            {

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
        if (currentPoint.isWarpPoint && !currentPoint.hasWarped)
        {

            if (currentPoint.autoWarp && !currentPoint.isLocked)
                StartCoroutine(TeleportPlayer(teleportTime));
        }

        if (currentPoint.isCorner && currentPoint.isWarpPoint)
        {

            CheckInput();
            SelectLevel();
        }

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
            transform.position = buildingLevel.transform.position;

            spriteRenderer.enabled = true;
            currentPoint = buildingLevel;
            prevPoint = currentPoint;

            canMove = true;
        }
        if (DataManager.instance.gameData.currentLevelName == "Building")
        {
            transform.position = buildingLevel.transform.position;

            spriteRenderer.enabled = true;
            currentPoint = buildingLevel;
            prevPoint = currentPoint;

            canMove = true;
        }
        if (DataManager.instance.gameData.currentLevelName == "Desert")
        {
            transform.position = desertLevel.transform.position;

            spriteRenderer.enabled = true;
            currentPoint = desertLevel;
            prevPoint = currentPoint;

            canMove = true;
        }
        if (DataManager.instance.gameData.currentLevelName == "Forest")
        {
            transform.position = forestLevel.transform.position;

            spriteRenderer.enabled = true;
            currentPoint = forestLevel;
            prevPoint = currentPoint;

            canMove = true;
        }
        /*else
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
        }*/
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
                    GameManager._currentState = GameManager.GameState.Entering;
                    SceneManager.LoadScene(currentPoint.sceneToLoad);
                }
                else if (currentPoint.isWarpPoint && !currentPoint.autoWarp && !currentPoint.isLocked)
                {
                    //teleport animation

                    StartCoroutine(TeleportPlayer(teleportTime));
                }
            }
        }
    }

    IEnumerator TeleportPlayer(float time)
    {
        currentPoint.hasWarped = true;
        canMove = false;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
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

    void OnEnable()
    {
        //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable()
    {
        //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        SetPlayerPos();
    }

    #endregion

}
