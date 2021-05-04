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
        //PlayerPrefs.DeleteAll();
    }

    private void LoadGameScene()
    {
        int indexScene = PlayerPrefs.GetInt("0");
        int numLevels = PlayerPrefs.GetInt("1");
        YandexCustomEvent.LevelStart(numLevels);
        if (indexScene == 0)
        {
            indexScene += 1;
            PlayerPrefs.SetInt("0", indexScene);
            PlayerPrefs.SetInt("1", indexScene);
        }
        //Debug.Log("numLevel" + indexScene);
        SceneManager.LoadScene(indexScene);
    }

}   
