using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int normalSpeed = 5; public int highSpeed; public int forwardSpeed; public float laneDistance = 2.5f;
    public float jumpForce = 5f;
    public float leftBoundary = -5f;
    public float rightBoundary = 5f;
    private bool isGrounded = true;
    public bool onBurningTile = false; public bool isHighSpeed = false;
    public Rigidbody rb;
    public int targetLane = 0;
    public Vector3 targetPosition;

    public GameObject gameOverPanel;
    public GameObject pauseMenuPanel; public SpeedManager speedManager;
    public bool isGameOver = false;
    private bool isPaused = false; public float fallThreshold = -10f;

    public FuelManager fuelManager;

    public AudioSource audioSource;
    public AudioClip boostSound;
    public AudioClip stickySound;
    public AudioClip suppliesSound;
    public AudioClip burningSound;
    public AudioClip obstacleSound;
    public AudioClip fallSound;
    public AudioClip invalidSound;
    public AudioClip soundtrack;
    public AudioClip panelSoundtrack;
    private AudioSource panelAudioSource;
    public Text finalScoreText;
    public ScoreManager scoreManager;

    private bool fallSoundPlayed = false;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetPosition = transform.position;
        forwardSpeed = normalSpeed; highSpeed = normalSpeed * 2; AudioSource[] audioSources = GetComponents<AudioSource>();
        audioSource = audioSources[0]; panelAudioSource = audioSources[1];

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        panelAudioSource.Stop();
        if (audioSource != null && soundtrack != null)
        {
            audioSource.clip = soundtrack;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        if (isPaused) return;
        transform.Translate(Vector3.forward * forwardSpeed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (targetLane <= -1)
            {
                PlayInvalidSound();
            }
            else
            {
                targetLane--;
            }
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (targetLane >= 1)
            {
                PlayInvalidSound();
            }
            else
            {
                targetLane++;
            }
        }

        targetPosition = new Vector3(targetLane * laneDistance, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * forwardSpeed);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else
            {
                PlayInvalidSound();
            }
        }

        if (transform.position.y < 0 && !fallSoundPlayed)
        {
            if (audioSource != null && fallSound != null)
            {
                audioSource.PlayOneShot(fallSound);
                fallSoundPlayed = true;
            }
        }

        if (transform.position.y < fallThreshold)
        {
            GameOver();
        }
    }

    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        isGrounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            fallSoundPlayed = false;
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BurningTile"))
        {
            onBurningTile = true;

            if (audioSource != null && burningSound != null)
            {
                audioSource.PlayOneShot(burningSound);
            }

        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("BoostTile"))
        {
            if (!isHighSpeed)
            {
                forwardSpeed = highSpeed;
                isHighSpeed = true;

                if (audioSource != null && boostSound != null)
                {
                    audioSource.PlayOneShot(boostSound);
                }
                if (speedManager != null)
                {
                    speedManager.UpdateSpeedStatus();
                }
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("StickyTile"))
        {
            if (isHighSpeed)
            {
                forwardSpeed = normalSpeed;
                isHighSpeed = false;
            }

            if (audioSource != null && stickySound != null)
            {
                audioSource.PlayOneShot(stickySound);
            }
            if (speedManager != null)
            {
                speedManager.UpdateSpeedStatus();
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("SuppliesTile"))
        {
            if (fuelManager != null)
            {
                fuelManager.ResetFuel();
            }

            if (audioSource != null && suppliesSound != null)
            {
                audioSource.PlayOneShot(suppliesSound);
            }
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            forwardSpeed = 0; rb.velocity = Vector3.zero;
            if (audioSource != null && obstacleSound != null)
            {
                audioSource.PlayOneShot(obstacleSound);
            }

            Invoke(nameof(GameOver), 1.0f);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("BurningTile"))
        {
            onBurningTile = false;
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        forwardSpeed = 0;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        if (panelAudioSource != null && panelSoundtrack != null)
        {
            panelAudioSource.clip = panelSoundtrack;
            panelAudioSource.loop = true; 
            panelAudioSource.Play();
        }
        if (finalScoreText != null && scoreManager != null)
        {
            finalScoreText.text = "Final Score: " + Mathf.FloorToInt(scoreManager.GetScore()).ToString();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f; forwardSpeed = normalSpeed;
        isGameOver = false;
        isPaused = false;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PlayInvalidSound()
    {
        if (audioSource != null && invalidSound != null)
        {
            audioSource.PlayOneShot(invalidSound);
        }
    }

    public void PauseGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }
        Time.timeScale = 0f; isPaused = true;

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Pause();
        }

        if (panelAudioSource != null && panelSoundtrack != null && !panelAudioSource.isPlaying)
        {
            panelAudioSource.clip = panelSoundtrack;
            panelAudioSource.loop = true; panelAudioSource.Play();
        }
    }

    public void ResumeGame()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        Time.timeScale = 1f; isPaused = false;

        if (panelAudioSource != null && panelAudioSource.isPlaying)
        {
            panelAudioSource.Stop();
        }

        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("TitleScreen");
    }



}