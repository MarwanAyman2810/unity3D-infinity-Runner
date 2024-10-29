using UnityEngine;
using UnityEngine.UI;

public class SpeedManager : MonoBehaviour
{
         public PlayerController playerController;

         public Text speedStatusText;

    void Update()
    {
        UpdateSpeedStatus();
    }

    public void UpdateSpeedStatus()
    {
                 if (playerController.isHighSpeed)
        {
            speedStatusText.text = "High Speed!";
            speedStatusText.color = Color.red;          }
        else
        {
            speedStatusText.text = "Normal Speed";
            speedStatusText.color = Color.green;          }
    }
}
