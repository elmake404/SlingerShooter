using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimatorEnemyState
{
    run,
    attack,
    walk,
    idle
}

public class EnemyAnimation : MonoBehaviour
{
    private int walkHash;
    private int attackHash;
    private int idleHash;
    private Animator animator;
    private AnimatorEnemyState animatorState;

    private void Start()
    {
        animator = GetComponent<Animator>();
        walkHash = Animator.StringToHash("isWalk");
        idleHash = Animator.StringToHash("isIdle");
        attackHash = Animator.StringToHash("isAttack");
    }
    private void SetAttackAnimation()
    {
        animator.SetBool("isAttacking", true);
    }

    private void ActivateAnimatorEnemyState(AnimatorEnemyState animatorEnemyState)
    {
        switch (animatorEnemyState)
        {
            case AnimatorEnemyState.run:
                break;
            case AnimatorEnemyState.attack:
                animator.SetBool(attackHash, true);
                break;
            case AnimatorEnemyState.walk:
                animator.SetBool(idleHash, false);
                animator.SetBool(walkHash, true);
                break;
            case AnimatorEnemyState.idle:
                animator.SetBool(idleHash, true);
                break;

        }
    }

    public void ChangeAnimationState(AnimatorEnemyState animatorEnemyState)
    {
        if (animatorEnemyState == animatorState) { return; }

        animatorState = animatorEnemyState;
        ActivateAnimatorEnemyState(animatorState);
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void EnableAnimator()
    {
        animator.enabled = true;
    }

    
}
