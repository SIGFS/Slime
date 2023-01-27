using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MapPoint : MonoBehaviour
{
    #region Variables

    //Waypoints
    [Header("Waypoints")]
    public MapPoint up;
    public MapPoint right, down, left;

    //Scene Options
    [Header("Scene Options")]
    [SerializeField] int levelIndex = 0;
    [HideInInspector] public string sceneToLoad;
    [TextArea(1, 2)]
    public string levelName;

    //Map Point Options
    [Header("Map Point Options")]
    [HideInInspector] public bool isLocked;
    public bool isLevel;
    public bool isCorner;
    public bool isWarpPoint;

    //Warp Options
    [Header("Warp Options")]
    public bool autoWarp;
    [HideInInspector] public bool hasWarped;
    public MapPoint warpPoint;

    //Image Options
    [Header("Image Options")]
    [SerializeField] Sprite unlockedSprite = null;
    [SerializeField] Sprite lockedSprite = null;

    //Level UI Objects
    [Header("Level UI Objects")]
    [SerializeField] TextMeshProUGUI levelText = null;
    [SerializeField] GameObject levelPanel = null;

    SpriteRenderer spriteRenderer;
    #endregion

    #region Unity Methods
    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if(levelPanel != null)
        {
            levelPanel.SetActive(false);
        }

        //if not a level or warp point set image to null
        if(!isLevel && lockedSprite != null)
        {
            if (isLocked && lockedSprite != null)
                spriteRenderer.sprite = lockedSprite;
            else
                spriteRenderer.sprite = null;
        }
        else
        {
            if (isLevel)
            {
                //Data Management
            }

            if (isLocked)
            {
                if (spriteRenderer.sprite != null)
                    spriteRenderer.sprite = lockedSprite;
            }
            else
            {
                if (spriteRenderer.sprite != null)
                    spriteRenderer.sprite = unlockedSprite;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (isLocked)
            {
                if (levelPanel != null)
                    levelPanel.SetActive(true);

                if (levelText != null)
                    levelText.text = "Level locked";
            }
            else
            {
                if (levelPanel != null)
                    levelPanel.SetActive(true);

                if (levelText != null)
                    levelText.text = levelName;
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (levelPanel != null)
                levelPanel.SetActive(false);

            hasWarped = false;

            if (levelText != null)
                levelText.text = "";
        }
    }
    #endregion
}
