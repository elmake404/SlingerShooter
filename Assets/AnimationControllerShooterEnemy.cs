using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationStateEnemyShooter
{
    isIdle,
    isShoot
}

public class AnimationControllerShooterEnemy : MonoBehaviour
{
    private Animator animator;
    private int idleAnimation;
    private int shootAnimation;
    private float durationShootAnimation = 1f;
    private AnimationStateEnemyShooter currentAnimationState = AnimationStateEnemyShooter.isIdle;

    void Start()
    {
        animator = GetComponent<Animator>();
        idleAnimation = Animator.StringToHash("isIdle");
        shootAnimation = Animator.StringToHash("isShoot");
        
    }

    public void SetAnimation(AnimationStateEnemyShooter animationState)
    {
        if (animationState == currentAnimationState) { return; }

        switch (animationState)
        {
            case AnimationStateEnemyShooter.isIdle:
                animator.SetBool(shootAnimation, false);
                currentAnimationState = AnimationStateEnemyShooter.isIdle;
                break;
            case AnimationStateEnemyShooter.isShoot:
                animator.SetBool(shootAnimation, true);
                currentAnimationState = AnimationStateEnemyShooter.isShoot;
                StartCoroutine(SetNextIdleAnimation());
                break;
        }
    }

    public void DisableAnimator()
    {
        animator.enabled = false;

    }

    private IEnumerator SetNextIdleAnimation()
    {
        yield return new WaitForSeconds(durationShootAnimation);
        SetAnimation(AnimationStateEnemyShooter.isIdle);
        yield return null;
    }
}
