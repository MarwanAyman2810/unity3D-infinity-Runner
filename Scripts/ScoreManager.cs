using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public float score = 0f; public Text scoreText; public PlayerController playerController;
    void Update()
    {
        if (!playerController.isGameOver)
        {
            score += Time.deltaTime; scoreText.text = "Score: " + Mathf.FloorToInt(score).ToString();
        }
    }
    public float GetScore()
    {
        return score;
    }
}
