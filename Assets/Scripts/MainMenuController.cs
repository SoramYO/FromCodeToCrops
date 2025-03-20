using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Add this for UI components

public class MainMenuController : MonoBehaviour
{
    public string levelToStart;
    public string guideToStart;
    public string mainMenu;
    public Button continueButton; // Reference to the Continue button

    private void Start()
    {
        AudioManager.instance.StopAllMusic(); // Dừng tất cả nhạc trước
        AudioManager.instance.PlayTitle(); // Phát nhạc menu chính

        // Check if save file exists and hide/show Continue button accordingly
        string path = Application.persistentDataPath + "/savegame.json";
        if (continueButton != null)
        {
            continueButton.gameObject.SetActive(File.Exists(path));
        }
    }

    public void PlayGame()
    {
        AudioManager.instance.StopAllMusic(); // Dừng nhạc menu
        AudioManager.instance.PlayNextBGM(); // Phát nhạc gameplay
        AudioManager.instance.PlaySFXPitchAdjusted(5);
        SceneManager.LoadScene(levelToStart);
    }

    public void GuideGame()
    {
        SceneManager.LoadScene(guideToStart);
        AudioManager.instance.PlaySFXPitchAdjusted(5);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(mainMenu);
        AudioManager.instance.PlaySFXPitchAdjusted(5);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting The Game");
        AudioManager.instance.PlaySFXPitchAdjusted(5);
    }

    public void NewGame()
    {
        // Xóa file save nếu cần
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
            File.Delete(path);
        SceneManager.LoadScene("Main");
    }
    public void ContinueGame()
    {
        string path = Application.persistentDataPath + "/savegame.json";
        if (File.Exists(path))
        {
            // Register a callback so that when Main scene finishes loading, we load the saved data.
            SceneManager.sceneLoaded += OnMainSceneLoaded;
            SceneManager.LoadScene("Main");
        }
        else
        {
            Debug.Log("No save game file found!");
            if (UIController.instance != null)
                UIController.instance.ShowMessage("Save game file not found!");
        }
    }
    private void OnMainSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            // Ensure the SaveManager instance exists now.
            if (SaveManager.instance != null)
            {
                SaveManager.instance.LoadGame();
            }
            else
            {
                Debug.LogError("SaveManager.instance is null! Make sure a SaveManager is present in the scene or set as persistent.");
            }
            // Remove the callback so it does not fire again.
            SceneManager.sceneLoaded -= OnMainSceneLoaded;
        }
    }
}