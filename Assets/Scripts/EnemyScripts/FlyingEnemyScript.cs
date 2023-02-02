using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyScript : MonoBehaviour
{
    public float speed;

    public GameObject enemy;
    public Transform leftGate;
    public Transform rightGate;

    public SpriteRenderer enemySprite;
    public BoxCollider2D enemyBox;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
        if(enemy.transform.position.x < leftGate.position.x)
        {
            
            enemy.transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }
}
