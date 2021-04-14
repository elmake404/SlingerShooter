using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScreenControl : MonoBehaviour
{
    private Vector2 previousDeltaPos = Vector2.zero;
    public static float inputVelocity = 0f;
    public static Vector2 inputVectorVelocity = Vector2.zero;
    public static Vector2 inputDirection = Vector2.zero;
    public static Vector2 inputVectorFromStart;
    private Vector2 startVector;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startVector = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if (Input.GetMouseButton(0))
        {
            inputDirection = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - previousDeltaPos;
            inputVelocity = Vector2.Distance(Vector2.zero, previousDeltaPos);
            inputVectorVelocity = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - previousDeltaPos;
            previousDeltaPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            inputVectorFromStart = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - startVector;
            
        }
        if (Input.GetMouseButtonUp(0))
        {
            previousDeltaPos = Vector2.zero;
            inputVelocity = 0f;
            inputDirection = Vector2.zero;
            inputVectorFromStart = Vector2.zero;
        }
    }
}
