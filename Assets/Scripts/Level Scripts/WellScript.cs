using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WellScript : MonoBehaviour
{
    public float liquidRemaining;
    private float delay;
    private float delayMax = 1f;
    BoxCollider2D well;

    void Start() {
        well = GetComponent<BoxCollider2D>();
        delay = 1f;
    }

    void Update() {
        delay += Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        //checks if the collider colliding with it is attached to the player, if there is enough liquid remaining, if the cooldown is up, and if the player is
        //not at the max size. If so, increases the player's size
        if(collider.tag == "Player" && liquidRemaining > 0 && delay >= delayMax && !collider.gameObject.GetComponent<SizeScript>().isMaxSize()) {
            delay = 0f;
            liquidRemaining--;
            transform.localScale -= new Vector3(0f, 0.34f, 0f);
        }
    }
}
