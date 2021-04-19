using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateOnlyPuulingBehaviour : StateMachineBehaviour
{
    private PlayerController playerController;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController = animator.transform.parent.GetComponent<PlayerController>();
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerController.InvokeCurrentShootVariat();
        playerController.MakeProjectile();
    }


}
