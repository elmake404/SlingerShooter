﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CanvasManager : MonoBehaviour
{
    public Button restartButton;
    public GameObject restartInDead;
    public GameObject levelComplete;
    public Button restarButtonInDead;
    public Button nextLevelButton;
    public Text textCompletedLevel;
    private int currentSceneBuildIndex;
    private int allSceneCount;
    private SavedStatus savedStatus;

    void Start()
    {
        allSceneCount = SceneManager.sceneCountInBuildSettings;
        restartButton.onClick.AddListener(RestartScene);
        restarButtonInDead.onClick.AddListener(RestartScene);
        nextLevelButton.onClick.AddListener(LoadNextLevel);
        currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;

        
        savedStatus = new SavedStatus("0", "1");
       
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(currentSceneBuildIndex);
    }

    public void InitRestartMenu()
    {
        restartInDead.SetActive(true);
    }

    public void InitLevelComplete()
    {
        SetTextCompleteLevel();
        levelComplete.SetActive(true);
    }

    private void LoadNextLevel()
    {
        if (currentSceneBuildIndex + 1 >= allSceneCount)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(currentSceneBuildIndex + 1);
        }
        AddAndSaveSceneIndex();
        AddAndSaveNumOfCompletedLevel();
    }

    private void SetTextCompleteLevel()
    {
        textCompletedLevel.text = "Level " + PlayerPrefs.GetInt(savedStatus.key_numOfCompleted) + "\nCompleted!";
    }

    private void AddAndSaveNumOfCompletedLevel()
    {
        int saved = PlayerPrefs.GetInt(savedStatus.key_numOfCompleted);
        saved += 1;
        Debug.Log("AddAndSaveNum  " + saved);
        PlayerPrefs.SetInt(savedStatus.key_numOfCompleted, saved);
    }

    private void AddAndSaveSceneIndex()
    {
        int saved = PlayerPrefs.GetInt(savedStatus.key_lastindexScene);
        if (saved + 1 >= currentSceneBuildIndex)
        {
            saved = 0;
        }
        else
        {
            saved += 1;
        }
        Debug.Log("AddAndSaveSceneIndex  " + saved);
        PlayerPrefs.SetInt(savedStatus.key_lastindexScene, saved);
    }

}

 struct SavedStatus
{
    public int indexScene;
    public int numOfCompletedLevel;
    public string key_lastindexScene;
    public string key_numOfCompleted;

    public SavedStatus(string lastIndex, string totalCount) : this()
    {
        key_lastindexScene = lastIndex;
        key_numOfCompleted = totalCount;
    }
}