using UnityEngine;
using UnityEngine.UI;

public class FuelManager : MonoBehaviour
{
    public float fuel = 50f;
    public float maxFuel = 50f;
    public Text fuelText;
    public PlayerController playerController;

    void Start()
    {
        fuel = maxFuel;
    }

    void Update()
    {
        if (!playerController.isGameOver)
        {
                         if (playerController.onBurningTile)
            {
                fuel -= 10f * Time.deltaTime;
            }
            else
            {
                                 fuel -= 1f * Time.deltaTime;
            }

                         fuel = Mathf.Clamp(fuel, 0f, maxFuel);

                         if (fuelText != null)
            {
                fuelText.text = "Fuel: " + Mathf.FloorToInt(fuel).ToString();
            }

                         if (fuel <= 0f)
            {
                playerController.GameOver();
            }
        }
    }

         public void ResetFuel()
    {
        fuel = maxFuel;

                 if (fuelText != null)
        {
            fuelText.text = "Fuel: " + Mathf.FloorToInt(fuel).ToString();
        }
    }
}
