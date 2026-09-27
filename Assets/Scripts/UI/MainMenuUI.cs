using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public void PlayGame()
    {
        if (SceneController.Instance != null)
        {
            SceneController.Instance.LoadSceneByName("Lobby");
        }
    }

    public void ExitGame()
    {
        if (SceneController.Instance != null)
        {
            SceneController.Instance.ExitGame();
        }
    }
}