using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAfter : MonoBehaviour
{
    public Animator anim;
    // Update is called once per frame
    public void PlayAfterEnd()
    {
        anim.Play("battery");
    }
}