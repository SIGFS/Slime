using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DennisScript : MonoBehaviour
{
    #region Declarations

    private float force = 2.5f;

    [Header("Instantiate")]
    public GameObject slimeBullet;
    public GameObject centerObject;

    private Transform centerPos;
    private float distance;
    private bool inBossFight;

    private Vector2 dir;
    private GameObject Dennis;
    private GameObject Player;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        Dennis = GameObject.FindGameObjectWithTag("Dennis");
        Player = GameObject.FindGameObjectWithTag("Player");
        distance = (Dennis.GetComponent<CircleCollider2D>().bounds.extents.x * 0.66f);
    }
    // Start is called before the first frame update
    void Start()
    {
        inBossFight = false;
        centerPos = centerObject.transform;
        StartCoroutine(BossFight());
    }

    // Update is called once per frame
    void Update()
    {
        //direction calculation
        Vector2 CurPos = Player.transform.position;
        Vector2 SlimePos = transform.position;
        dir = CurPos - SlimePos;

        PointAtCursor(dir);
        if (Vector3.Distance(transform.position, centerPos.position) > distance)
        {
            distance = Dennis.GetComponent<CircleCollider2D>().bounds.extents.x * 0.5f;
            Vector3 directon = (transform.position - centerPos.position).normalized * distance;
            transform.position = centerPos.position + directon;
        }
    }
    #endregion

    #region User Methods
    //rotating to aim at the mouse
    void PointAtCursor(Vector2 dir)
    {
        transform.Translate(dir);
    }

    void Shoot(Vector2 dir)
    {
        if (!SizeScript.instance.isMinSize())
        {
            GameObject BallIns = Instantiate(slimeBullet, transform.position, Quaternion.identity);
            BallIns.GetComponent<Rigidbody2D>().velocity = dir * force;

        }
    }

    public void EnterBossFight()
    {
        inBossFight = true;
    }
    #endregion

    #region Coroutines
    public IEnumerator BossFight()
    {
        while (inBossFight)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));
            Shoot(dir);
        }
        yield return null;
    }

    #endregion
}
