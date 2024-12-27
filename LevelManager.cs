using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void ReplayLevel()
    {
        // Reload the current active scene (replays the level without resetting UI)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Return to the home menu (simulates restarting the game)
    public void GoHome()
    {
       
        // You can load the main menu scene if you have a separate one, otherwise reload the current scene but reset the UI
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

   
    }

    // Move to the next level (load the next scene in the build settings)
    public void NextLevel()
    {
        // Load the next scene in the build index (make sure your levels are ordered in the build settings)
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

        // Check if the next scene index is valid, i.e., within the range of scenes in build settings
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No more levels to load!");
            // Optionally, handle the case when there are no more levels, e.g., show a "Congratulations" message or loop back.
        }
    }
}
