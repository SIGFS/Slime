using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SizeScript : MonoBehaviour
{
    #region Declarations
    [HideInInspector]public static SizeScript instance { get; set; }

    [SerializeField] private GameObject player;
    [SerializeField] private PlayerData Data;
    [Header("Position Initialization")]
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private GameObject leftWallCheck, rightWallCheck, floorCheck;

    private Animator playerAnim, attackPointAnim;
    private CircleCollider2D circleCollider;
    private Rigidbody2D playerRB;
    [SerializeField] private GameObject groundcheck; 
    #endregion

    #region Unity Methods
    private void Awake()
    {
        playerAnim = player.GetComponent<Animator>();
        attackPointAnim = attackPoint.GetComponent<Animator>();
        circleCollider = player.GetComponent<CircleCollider2D>();
        playerRB = player.GetComponent<Rigidbody2D>();

        if (instance == null)
        {
            instance = this;
        }
        Data.size = 3;
        //dataChange();

        playerAnim.SetInteger("Size", Data.size);
        attackPointAnim.SetInteger("Size", Data.size);
        UpdateCollider();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Spike")
        {
            SizeChangeDown();
        }
        if (collision.gameObject.tag == "DeathZone")
        {
            RestartCheckpoint();
        }
    }
    #endregion

    #region Size Adjustment
    public void SizeChangeUp()
    {
        if(!isMaxSize())
        {
            Data.size += 1;

            playerAnim.SetInteger("Size", Data.size);
            attackPointAnim.SetInteger("Size", Data.size);
            UpdateCollider();

            //Adjust position up to account for the change in size of object to prevent clipping through objects. Adjustment should be half of the size increase increment.
            transform.position += new Vector3(0f, 0.15f, 0f); 
            //dataChange();
        }

    }

    public void SizeChangeDown()
    {
        if(!isMinSize())
        {
            Data.size -= 1;

            
            playerAnim.SetInteger("Size", Data.size);
            attackPointAnim.SetInteger("Size", Data.size);
            UpdateCollider();

            //Adjust position down to put player back on floor. Adjustment should be half of the size decrease increment.
            transform.position -= new Vector3(0f, 0.15f, 0f); 
            //dataChange();

        }
        else if (isMinSize())
        {
            //Play slime explode fx
            this.gameObject.transform.parent.gameObject.SetActive(false);

            // For now, restart the scene. We need to add checkpoints later... 
            Invoke("RestartCheckpoint", 2f);
        }
    }
    #endregion

    #region Size Values
    public int getSize() {
        return Data.size;
    }

    public void setSize(int size) {
        Data.size = size;
    }

    public bool isMaxSize() {
        if(Data.size == 5) return true;
        return false;
    }

    public bool isMinSize() {
        if(Data.size == 1) return true;
        return false;
    }
    #endregion

    #region Data Update
    /*public void dataChange()
    {
        //formula

        //Data.jumpHeight = -.7777777f * getSize() + 7.7777777f;
        *//*Data.jumpHeight = -.55f * getSize() + 5.5f;
        Data.jumpTimeToApex = -.05f * getSize() + .55f;
        Data.runMaxSpeed = -1.1111f * getSize() + 17.111111f;
        Data.jumpForce = Mathf.Abs(Data.gravityStrength) * (-.05f * getSize() + .55f);*//*
    }*/

    void UpdateCollider()
    {
        /*
            Size 1: 0.1355778
            Size 2: 0.2299513
            Size 3: 0.3302285
            Size 4: 0.4305048
            Size 5: 0.5397078
             */

        switch (Data.size)
        {
            case 1:
                circleCollider.radius = 0.1355778f;
                playerRB.mass = .9f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(0.6f, 0.03f);
                break;
            case 2:
                circleCollider.radius = 0.2299513f;
                playerRB.mass = 1f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(1.05f, 0.03f);
                break;
            case 3:
                circleCollider.radius = 0.3302285f;
                playerRB.mass = 1.15f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(1.54f, 0.03f);
                break;
            case 4:
                circleCollider.radius = 0.4305048f;
                playerRB.mass = 1.3f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(2f, 0.03f);
                break;
            case 5:
                circleCollider.radius = 0.5397078f;
                playerRB.mass = 1.75f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(2.5f, 0.03f);
                break;
        }

        rightWallCheck.transform.position = new Vector3(circleCollider.bounds.max.x, player.transform.position.y, 0f);
        leftWallCheck.transform.position = new Vector3(circleCollider.bounds.min.x, player.transform.position.y, 0f);
        floorCheck.transform.position = new Vector3(player.transform.position.x, circleCollider.bounds.min.y, 0f);
    }

    #endregion
    void RestartCheckpoint()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }
}
