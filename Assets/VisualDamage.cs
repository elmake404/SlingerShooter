using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VisualDamage : MonoBehaviour
{
    public static VisualDamage instance;
    private Image damageTexture;
    private float maxAlpha = 140f;
    private Coroutine currentScreenDamage;


    void Start()
    {
        instance = this;
        damageTexture = transform.GetComponent<Image>();
    }

    public void InitDamage()
    {
        if (currentScreenDamage != null) 
        {
            //Debug.Log("StopCoroutine");
            StopCoroutine(currentScreenDamage);
        }

        currentScreenDamage = StartCoroutine(StartScreenDamage());
    }

    private IEnumerator StartScreenDamage()
    {
        float maxNumOfHits = DamageHits.instance.maxNumOfHits;
        float currentHits = DamageHits.instance.numOfCurrentDamageHits;
        float targetAlpha = Mathf.Lerp(0f, maxAlpha, currentHits / maxNumOfHits) / maxAlpha;
        Color changedColor = Color.white;

        for (float i = 0f; i <= currentHits; i += Time.deltaTime)
        {
            changedColor.a = Mathf.Lerp(targetAlpha, 0f, i/currentHits);
            //Debug.Log(changedColor.a);
            damageTexture.color = changedColor;

            yield return new WaitForEndOfFrame();
        }

        
        yield return null;
    }
}
