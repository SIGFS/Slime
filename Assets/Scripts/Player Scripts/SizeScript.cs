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
    private SpriteRenderer spriteRend;

    //Collider Variables
    float size1Radius = 0.1355778f;
    float size2Radius = 0.2280836f;
    float size3Radius = 0.3290744f;
    float size4Radius = 0.4249443f;
    float size5Radius = 0.5373184f;

    //Iframe info
    [SerializeField] private float IFrameDuration;
    [SerializeField] private float NumOfFlashes;

    Vector2 size1Offset = new Vector2(0.00480890274f, 0.0061096251f);
    Vector2 size2Offset = new Vector2(0.00480890274f, 0.00797730684f);
    Vector2 size3Offset = new Vector2(0.00480890274f, 0.00913137197f);
    Vector2 size4Offset = new Vector2(0.00480890274f, 0.0146918297f);
    Vector2 size5Offset = new Vector2(0.00480890274f, 0.0170812011f);
    #endregion

    #region Unity Methods
    private void Awake()
    {
        playerAnim = player.GetComponent<Animator>();
        attackPointAnim = attackPoint.GetComponent<Animator>();
        circleCollider = player.GetComponent<CircleCollider2D>();
        playerRB = player.GetComponent<Rigidbody2D>();
        spriteRend = player.GetComponent<SpriteRenderer>();

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
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeathZone")
        {
            gameObject.transform.parent.gameObject.SetActive(false);
            AudioManager.Instance.playDeath();
            Invoke(nameof(RestartCheckpoint), 2f);
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
            AudioManager.Instance.playGrow();
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
            AudioManager.Instance.playGrow();

        }
        else if (isMinSize())
        {
            //Play slime explode fx
            AudioManager.Instance.playDeath();
            gameObject.transform.parent.gameObject.SetActive(false);
            
            Invoke(nameof(RestartCheckpoint), 2f);
        }
    }
    #endregion

    #region IFrames

    public IEnumerator Invulnerability()
    {
        SizeChangeDown();
        Physics2D.IgnoreLayerCollision(3, 8, true);
        //Invuln Duration
        for (int i = 0; i < NumOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return  new WaitForSeconds(IFrameDuration / NumOfFlashes);
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(IFrameDuration / NumOfFlashes);
            
        }

        Physics2D.IgnoreLayerCollision(3, 8, false);
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

    public void UpdateCollider()
    {

        switch (Data.size)
        {
            case 1:
                circleCollider.radius = size1Radius;
                circleCollider.offset = size1Offset;

                playerRB.mass = .9f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(0.6f, 0.03f);
                break;
            case 2:
                circleCollider.radius = size2Radius;
                circleCollider.offset = size2Offset;

                playerRB.mass = 1f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(1.05f, 0.03f);
                break;
            case 3:
                circleCollider.radius = size3Radius;
                circleCollider.offset = size3Offset;

                playerRB.mass = 1.15f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(1.54f, 0.03f);
                break;
            case 4:
                circleCollider.radius = size4Radius;
                circleCollider.offset = size4Offset;

                playerRB.mass = 1.3f;
                player.GetComponent<PlayerMovement>()._groundCheckSize = new Vector2(2f, 0.03f);
                break;
            case 5:
                circleCollider.radius = size5Radius;
                circleCollider.offset = size5Offset;

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
