using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwapper : MonoBehaviour
{
    public static void LoadMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public static void LoadLevelMenu() {
        SceneManager.LoadScene("LevelScene");
    }

    public static void LoadTutorial() {
        SceneManager.LoadScene("TutorialOne");
    }

    public static void Quit() {
        Application.Quit();
    }
}