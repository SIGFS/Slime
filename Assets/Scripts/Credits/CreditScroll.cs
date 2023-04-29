using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditScroll : MonoBehaviour
{
    private RectTransform credits;
    public Image whiteS;
    public GameObject player;

    private bool scrollCreds;

    private void Awake()
    {
        credits = this.GetComponent<RectTransform>();
    }

    private void Start()
    {
        scrollCreds = false;
        StartCoroutine(FadeFromWhite());
    }

    void FixedUpdate()
    {
        player.transform.Translate(Vector3.right * 0.1f);
        
        
        if (scrollCreds)
        {
            if (credits.localPosition.y < 3383)
            {
                credits.localPosition += Vector3.up * 2;
            }
            else
            {
                StartCoroutine(FadeToWhite());
            }
        }
    }

    IEnumerator FadeToWhite()
    {
        float alphaValue = whiteS.color.a;
        Color tmp = whiteS.color;

        while (alphaValue < 1)
        {
            alphaValue += .08f;
            tmp.a = alphaValue;
            whiteS.color = tmp;
            yield return new WaitForSeconds(.05f);
        }
        if(alphaValue >= 1)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    IEnumerator FadeFromWhite()
    {
        float alphaValue = whiteS.color.a;
        Color tmp = whiteS.color;

        while (alphaValue > 0)
        {
            alphaValue -= .08f;
            tmp.a = alphaValue;
            whiteS.color = tmp;
            yield return new WaitForSeconds(.05f);
        }
        if (alphaValue <= 0)
        {
            scrollCreds = true;
        }
    }
}
