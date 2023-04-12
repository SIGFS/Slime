using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] private GameObject ConnectedBarrier;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void PressButton()
    {
        anim.SetBool("Pressed", true);
        ConnectedBarrier.SetActive(false);
    }
}
