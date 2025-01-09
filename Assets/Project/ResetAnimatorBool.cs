using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    public string isUsingRightHandBool = "isUsingRightHand";
    public bool isUsingRightHandStatus = true;
    public string isUsingLeftHandBool = "isUsingLeftHand";
    public bool isUsingLeftHandStatus = true;
    public string isInvulnerableBool = "isInvulnerable";
    public bool isInvulnerableStatus = false;
    public string isInteractingBool = "isInteracting";
    public bool isInteractingStatus = false;
    public string isFiringSpellBool = "isFiringSpell";
    public bool isFiringSpellStatus = false;
    public string isRotatingWithRootMotionBool = "isRotatingWithRootMotion";
    public bool isRotatingWithRootMotionStatus = false;
    public string canRotateBool = "canRotate";
    public bool canRotateStatus = true;
    public string isMirroedBool = "isMirroed";
    public bool isMirroedStatus = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(isInteractingBool, isInteractingStatus);
        animator.SetBool(isFiringSpellBool, isFiringSpellStatus);
        animator.SetBool(canRotateBool, canRotateStatus);
        animator.SetBool(isRotatingWithRootMotionBool, isRotatingWithRootMotionStatus);
        animator.SetBool(isInvulnerableBool, isInvulnerableStatus);
        animator.SetBool(isUsingRightHandBool, isUsingRightHandStatus);
        animator.SetBool(isUsingLeftHandBool, isUsingLeftHandStatus);
        animator.SetBool(isMirroedBool, isMirroedStatus);
    }
}
