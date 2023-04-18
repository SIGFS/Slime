using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    public Image whiteS;
    public GameObject barrier;
    public bool isExit;
    public bool isEnter;
    
    private GameObject player;
    private int size;



    private void FixedUpdate()
    {
        if(player != null)
        {
            if(GameManager._currentState == GameManager.GameState.Entering | GameManager._currentState == GameManager.GameState.Leaving)
            {
                player.transform.Translate(Vector3.right * 0.03f);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {

            if (GameManager._currentState == GameManager.GameState.Entering)
            {
                if(isEnter)
                    StartCoroutine(FadeFromWhite());

                if(barrier != null && !isExit)
                {
                    barrier.SetActive(true);
                }
                GameManager._currentState = GameManager.GameState.Level;

                return;
            }
            else if (GameManager._currentState == GameManager.GameState.Level && isExit)
            {
                player = collision.gameObject;
                player.GetComponent<PlayerMovement>().enabled = false;

                size = player.GetComponent<SizeScript>().getSize();
                player.GetComponent<Animator>().Play("Idle" + size);


                StartCoroutine(FadeToWhite());

                return;
            }

        }
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
            GameManager.instance.LevelWin();
        }
    }
    IEnumerator FadeFromWhite()
    {
        float alphaValue = 1;
        Color tmp = whiteS.color;
        

        while (alphaValue > 0)
        {
            alphaValue -= .01f;
            tmp.a = alphaValue;
            whiteS.color = tmp;
            yield return new WaitForSeconds(.03f);
        }
    }
}
