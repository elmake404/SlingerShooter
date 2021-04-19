using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShakeOffset 
{
    private float duration;
    private float halfDuration;
    private float multiplayer = 0f;
    private float maxMultiplayer = 100f;

    private float xVelocity = 0f;
    private float yVelocity = 0f;
    private float zVelocity = 0f;

    private float counter = 0f;
    private float totalCounter = 0f;
    private Vector3 currentVector;
    private Vector3 initVector;
    public bool isEnd = false;

    public ShakeOffset(Vector3 initDirection ,float setDuration)
    {
        currentVector = GetVectorNoised();
        duration = setDuration;
        halfDuration = duration / 2f;
        initVector = initDirection;
    }


    private void AddCounter()
    {
        counter += Time.deltaTime;
        if (counter > 0.1f)
        {
            currentVector = GetVectorNoised();
            counter = 0f;
        }

    }

    private void AddTotalCounter()
    {
        totalCounter += Time.deltaTime;

        if (totalCounter < halfDuration)
        {
            multiplayer = maxMultiplayer * (Mathf.InverseLerp(0f, halfDuration, totalCounter) / maxMultiplayer);

        }
        else
        {
            multiplayer = maxMultiplayer * (Mathf.InverseLerp(duration, halfDuration, totalCounter) / maxMultiplayer);
            
        }
        if (totalCounter > duration)
        {
            isEnd = true;
        }
    }

    private Vector3 GetVectorNoised()
    {
        if(totalCounter > duration - 0.15f) { return initVector; }

        float tick = UnityEngine.Random.Range(-100f, 100f);
        
        Vector3 offsetVector = Vector3.zero;
        offsetVector.x =  maxMultiplayer * (Mathf.PerlinNoise(tick, 0f) - 0.5f);
        offsetVector.y =  maxMultiplayer * (Mathf.PerlinNoise(0f, tick) - 0.5f);
        offsetVector.z =  maxMultiplayer * (Mathf.PerlinNoise(tick, tick) - 0.5f);
        return offsetVector;
    }

    public Vector3 GetSmoothedEulerAngles()
    {
        AddCounter();
        AddTotalCounter();
        float x = multiplayer * Mathf.SmoothDampAngle(initVector.x, currentVector.x, ref xVelocity, 0.1f);
        float y = multiplayer * Mathf.SmoothDampAngle(initVector.y, currentVector.y, ref yVelocity, 0.1f);
        float z = multiplayer * Mathf.SmoothDampAngle(initVector.z, currentVector.z, ref zVelocity, 0.1f);

        return new Vector3(x, y, z);
    }

}
