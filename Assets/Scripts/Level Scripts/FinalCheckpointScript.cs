using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalCheckpointScript : MonoBehaviour
{
    public Image whiteS;
    
    private GameObject player;
    private int size;



    private void FixedUpdate()
    {
        if(player != null)
        {
            player.transform.Translate(Vector3.right * 0.01f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            player = collision.gameObject;
            player.GetComponent<PlayerMovement>().enabled = false;
            size = player.GetComponent<SizeScript>().getSize();
            player.GetComponent<SizeScript>().enabled = false;
            StartCoroutine(DisableAnimator());
            StartCoroutine(FadeToWhite());
        }
    }

    IEnumerator DisableAnimator()
    {
        player.GetComponent<Animator>().Play("Idle" + size);
        yield return new WaitForEndOfFrame();
        player.GetComponent<Animator>().enabled = false;
    }

    IEnumerator FadeToWhite()
    {
        float alphaValue = whiteS.color.a;
        Color tmp = whiteS.color;

        while (alphaValue < 1)
        {
            alphaValue += .01f;
            tmp.a = alphaValue;
            whiteS.color = tmp;
            yield return new WaitForSeconds(.03f);
        }
        if (alphaValue >= 1)
        {
            //Return to overworld
        }
    }
}
