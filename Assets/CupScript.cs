using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupScript : MonoBehaviour
{
    [Range(0f, 1f)]
    public float fill;
    Animation anim;
    AnimationClip animclip;
    // Update is called once per frame
    private void Start()
    {
        anim = transform.GetChild(0).GetComponent<Animation>();
        animclip = anim.clip;
        anim[animclip.name].speed = 0f;
    }
    void Update()
    {
        anim[animclip.name].normalizedTime = fill;
    }
}
