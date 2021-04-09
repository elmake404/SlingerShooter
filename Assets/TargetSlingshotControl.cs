using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TargetControlState
{
    targetIsActive = 0,
    targetIsNonActive = 1,
    targetIsOnEnemy = 2
}

public class TargetSlingshotControl : MonoBehaviour
{
    [HideInInspector] public TargetControlState controlState = TargetControlState.targetIsNonActive;
    [HideInInspector] public Ray directionRayTarget;
    [HideInInspector] public Camera sceneCamera;
    public Color colorWhenTarget;
    [HideInInspector] public bool isTargetOnEnemy = false;
    private Image targetImage;
    private RectTransform rectTransform;
    private Color transperentColor;
    private Color normalColor;
    private Vector3 initialPos;
    private float speedDecreased = 5f;
    private Color currentColor;

    private void Start()
    {
        
        targetImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        InitializeTarget();
    }

    private void InitializeTarget()
    {
        initialPos = rectTransform.position;
        normalColor = targetImage.color;
        Color color = targetImage.color;
        color.a = 0.3f;
        transperentColor = color;
        currentColor = color;
        targetImage.color = color;
    }

    private void Update()
    {
        SelectActionFromInput(controlState);
        //  Debug.Log(currentColor);
    }

    public void SelectActionFromInput(TargetControlState targetControlState)
    {
        switch (targetControlState)
        {
            case TargetControlState.targetIsActive:
                MoveTarget();
                GenerateRayFromTarget();
                SetColorWhenTargetIsActive();

                break;
            case TargetControlState.targetIsNonActive:
                SetColorWhenTargetIsNonActive();
                break;
            case TargetControlState.targetIsOnEnemy:
                MoveTarget();
                GenerateRayFromTarget();
                SetColorWhenTargetEnemy();
                break;
        }
        
    }

    private void MoveTarget()
    {
        rectTransform.position += new Vector3(ScreenControl.inputDirection.x, ScreenControl.inputDirection.y, 0f)  * Time.deltaTime * ScreenControl.inputVelocity/speedDecreased;
    }

    private void GenerateRayFromTarget()
    {
        Vector3 currentPos = rectTransform.position;
        directionRayTarget = sceneCamera.ScreenPointToRay(currentPos);
        Debug.DrawRay(directionRayTarget.origin, directionRayTarget.direction*1000f, Color.yellow);
    }
    
    private void SetColorWhenTargetEnemy()
    {
        if (currentColor == colorWhenTarget) { return; }
        targetImage.color = colorWhenTarget;
        currentColor = colorWhenTarget;
    }

    private void SetColorWhenTargetIsActive()
    {
        if (currentColor == normalColor) { return; }
        targetImage.color = normalColor;
        currentColor = normalColor;
    }

    private void SetColorWhenTargetIsNonActive()
    {
        if (currentColor == transperentColor) { return; }
        targetImage.color = transperentColor;
        currentColor = transperentColor;
    }

    
}
