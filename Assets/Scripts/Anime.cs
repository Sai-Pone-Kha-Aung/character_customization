using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anime : MonoBehaviour
{
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void zoomin()
    {
        _anim.Play("ZoomIn", -1, 0f);
    }

    public void zoomout()
    {
        _anim.Play("Zoomout1", -1, 0f);
    }
}
