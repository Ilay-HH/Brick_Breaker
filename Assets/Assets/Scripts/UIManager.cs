using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text gameOverText;
    [SerializeField]
    private Text highScoreText;
    [SerializeField]
    private Button restartButton;
    private int score;
    private int highScore;

    [SerializeField]
    private Sprite muteImage;
    [SerializeField]
    private Sprite unmuteImage;
    private AudioSource audioSource;
    private bool isMusicPlaying = true;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Start()
    {
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        gameOverText.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        restartButton.gameObject.SetActive(false);
        score = 0;
        scoreText.text = "Score:" + score;
        audioSource.playOnAwake = true;
        audioSource.loop = true;
        audioSource.volume = 0.15f;
    }
    public void BackGroundMusicManager(Button whoCalled)
    {
        switch (isMusicPlaying)
        {
            case true:
                audioSource.Pause();
                whoCalled.image.sprite = unmuteImage;
                isMusicPlaying = false;
                break;
            case false:
                audioSource.UnPause();
                whoCalled.image.sprite = muteImage;
                isMusicPlaying = true;
                break;
        }
    }
    public void ScoreUp(int Amount)
    {
        score += Amount;
        scoreText.text = "Score:" + score;
    }
    public void GameOverRoutine()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        audioSource.Stop();
        if (score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
        highScoreText.text = "HighScore: " + PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.gameObject.SetActive(true);
    }
}
