using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSlingControl : MonoBehaviour
{
    [HideInInspector] public Animator animator;
    private int animShoot;
    private int animPuling;
    private int lastAnimation;

    private void Start()
    {
        animShoot = Animator.StringToHash("isShoot");
        animPuling = Animator.StringToHash("isPuling");
        animator = transform.GetComponent<Animator>();
    }

    public void SetAnimationShot()
    {
        if (lastAnimation == animShoot) { return; }
        animator.SetBool(animPuling, false);
        animator.SetBool(animShoot, true);
        lastAnimation = animShoot;
    }

    public void SetAnimationPuling()
    {
        if (lastAnimation == animPuling) { return; }
        animator.SetBool(animShoot, false);
        animator.SetBool(animPuling, true);
        lastAnimation = animPuling;
    }
}
