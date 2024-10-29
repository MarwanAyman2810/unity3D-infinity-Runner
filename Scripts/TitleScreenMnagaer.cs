

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip titleScreenMusic;

         public GameObject optionsPanel;
    public Button muteButton;
    private bool isMuted = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

                 if (audioSource != null && titleScreenMusic != null)
        {
            audioSource.clip = titleScreenMusic;
            audioSource.loop = true;              audioSource.Play();
        }

                 optionsPanel.SetActive(false);

                 UpdateMuteButtonText();
    }

         public void StartGame()
    {
        Debug.Log("Start Game button pressed");          SceneManager.LoadScene("SampleScene");      }

         public void ToggleOptionsPanel()
    {
        Debug.Log("Options button pressed");          optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

         public void ToggleMute()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0 : 1;
        UpdateMuteButtonText();
    }

    private void UpdateMuteButtonText()
    {
        muteButton.GetComponentInChildren<Text>().text = isMuted ? "Unmute" : "Mute";
    }

         public void CloseOptionsPanel()
    {
        optionsPanel.SetActive(false);
    }
    public void QuitGame()
    {
        Debug.Log("Quit Game button pressed");  
                 Application.Quit();

             }

}








