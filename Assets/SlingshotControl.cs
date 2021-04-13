using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SlinghsotState
{
    slingshotIsActive,
    slingshotIsNonActive
}

public class SlingshotControl : MonoBehaviour
{
    //public Ray rayDirection;
    [HideInInspector] public TargetSlingshotControl targetControl;
    [HideInInspector] public SlinghsotState slinghsotState = SlinghsotState.slingshotIsNonActive;
    //public Camera sceneCamera;

    private void FixedUpdate()
    {
        SlingshotState(slinghsotState);
    }
    public void SlingshotState(SlinghsotState slinghsotState)
    {
        switch (slinghsotState)
        {
            case SlinghsotState.slingshotIsActive:
                RotateToDirection();
                break;
            case SlinghsotState.slingshotIsNonActive:
                break;
        }
    }

    private void RotateToDirection()
    {
        if (ScreenControl.inputVelocity <= 0.05f)
        { return; }
        float yNegative = targetControl.directionRayTarget.direction.y / Mathf.Abs( targetControl.directionRayTarget.direction.y);
        float xNegative = targetControl.directionRayTarget.direction.x / Mathf.Abs( targetControl.directionRayTarget.direction.x);
        Vector3 StartVector = new Vector3(-1f, 0f, 0f);
        Vector3 yDirVector = new Vector3(-1f, targetControl.directionRayTarget.direction.y, 0f);
        Vector3 xDirVector = new Vector3(-1f, 0f, targetControl.directionRayTarget.direction.x);
        float yDotProduct = Vector3.Dot(yDirVector, StartVector);
        float xDotProduct = Vector3.Dot(xDirVector, StartVector);
        float yMagnitudes = Vector3.Distance( Vector3.zero,StartVector) * Vector3.Distance(Vector3.zero,yDirVector);
        float xMagnitudes = Vector3.Distance( Vector3.zero,StartVector) * Vector3.Distance(Vector3.zero,xDirVector);
        float yToCos = yDotProduct / yMagnitudes;
        float xToCos = xDotProduct / xMagnitudes;
        float angleY = Mathf.Acos(yToCos) * Mathf.Rad2Deg * yNegative;
        float angleX = Mathf.Acos(xToCos) * Mathf.Rad2Deg * xNegative;
        
        //Debug.Log(targetControl.directionRayTarget.direction);
        transform.localRotation = Quaternion.Euler(new Vector3(0f, angleX, -angleY));

    }


}
