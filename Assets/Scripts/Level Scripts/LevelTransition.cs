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
    public bool isForest;
    
    private GameObject player;
    private int size;

    [SerializeField] private AudioSource music;
    [SerializeField] private AudioSource levelClear;



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
            player = collision.gameObject;
            //music.Stop();
            //levelClear.Play();

            if (GameManager._currentState == GameManager.GameState.Entering)
            {
                if (isEnter)
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
                player.GetComponent<PlayerMovement>().enabled = false;

                StartCoroutine(DisableAnimate());

                StartCoroutine(FadeToWhite());

                return;
            }

        }
    }

    IEnumerator DisableAnimate()
    {
        size = player.GetComponent<SizeScript>().getSize();
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
            yield return new WaitForSeconds(.02f);
        }
        if (alphaValue >= 1)
        {
            player.GetComponent<Animator>().enabled = true;
            //Return to overworld
            if (!isForest)
            {
                GameManager.instance.LevelWin();
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Credits");
            }
            
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
            yield return new WaitForSeconds(.02f);
        }
    }
}
