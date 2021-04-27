using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InitZeroScene : MonoBehaviour
{
    void Start()
    {
        LoadGameScene();
        FacebookManager.Instance.GameStart();
    }

    private void LoadGameScene()
    {
        int indexScene = PlayerPrefs.GetInt("0");
        int numLevels = PlayerPrefs.GetInt("1");
        YandexCustomEvent.LevelStart(numLevels);
        SceneManager.LoadScene(indexScene);
    }

}   
