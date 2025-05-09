using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinCameraAnim : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void skinzoomin()
    {
        anim.Play("Skin", -1, 0f);
    }
}
