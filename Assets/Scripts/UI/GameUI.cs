using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject victoryPanel;

    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.RegisterGameOverPanel(gameOverPanel);
            GameManager.instance.RegisterVictoryPanel(victoryPanel);
        }
    }
}