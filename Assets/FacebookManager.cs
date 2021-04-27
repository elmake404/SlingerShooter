using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Include Facebook namespace
using Facebook.Unity;


public class FacebookManager : MonoBehaviour
{
    public static FacebookManager Instance;
    void Awake()
    {
        Instance = this;

        if (!FB.IsInitialized)
        {
            FB.Init(InitCallback, OnHideUnity);
        }
        else
        {
            FB.ActivateApp();
        }
        DontDestroyOnLoad(this.gameObject);

    }

    private void InitCallback()
    {
        if (FB.IsInitialized)
        {
            // Signal an app activation App Event
            FB.ActivateApp();
            // Continue with Facebook SDK
            // ...
        }
        else
        {
            Debug.Log("Failed to Initialize the Facebook SDK");
        }
    }

    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            // Pause the game - we will need to hide
            Time.timeScale = 0;
        }
        else
        {
            // Resume the game - we're getting focus again
            Time.timeScale = 1;
        }
    }


    public void GameStart()
    {
        var tutParms = new Dictionary<string, object>();
        tutParms["Game start"] = "The game was launched";
        Debug.Log("GameStart()");
        FB.LogAppEvent(
            "Game start",
            parameters: tutParms);
    }
    public void LevelStart(int lvl)
    {
        var tutParms = new Dictionary<string, object>();
        tutParms["Level Namber"] = lvl.ToString();
        Debug.Log("Level Start " + tutParms["Level Namber"]);
        FB.LogAppEvent(
            "Level start",
            parameters: tutParms);
    }
    public void LevelWin(int lvl)
    {
        var tutParms = new Dictionary<string, object>();
        tutParms["Level Namber"] = lvl.ToString();
        Debug.Log("Level Win " + tutParms["Level Namber"]);
        FB.LogAppEvent(
            "Level win",
            parameters: tutParms);
    }

    public void LevelFail(int lvl)
    {
        var tutParms = new Dictionary<string, object>();
        tutParms["Level Namber"] = lvl.ToString();
        Debug.Log("Level Fail " + tutParms["Level Namber"]);
        FB.LogAppEvent(
            "Level fail",
            parameters: tutParms);
    }



}
