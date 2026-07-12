using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GameManager : MonoBehaviour
{
    private int score;
    public float spawnRate = 0.8f;
    public bool isGameActive;
    public int health = 3;
    public int maxHealth = 3;

    public List<ObjectPool> targetPools;    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI gameOverText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI finalScoreText;
    public AudioSource gameOverSound;
    public AudioSource backgroundMusic;
    public Button restartButton; 
    public GameObject titleScreen;
    public GameObject hearts;
    public GameObject gameOverScreen;
    public AudioSource loseLifeSound;
    public static GameManager Instance;

    // void Start()
    // {
    //     gameOverSound = GetComponent<AudioSource>();
    // }

    IEnumerator SpawnTarget(){
        while(isGameActive){
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPools.Count);
            // Instantiate(targets[index]);
            GameObject target = targetPools[index].GetObject();

        }
    }

    public void UpdateScore(int scoreToAdd){
        score += scoreToAdd;
        scoreText.text = "Score: " + score;
    }

    public void GameOver(){
        gameOverScreen.gameObject.SetActive(true);
        isGameActive = false;
        scoreText.gameObject.SetActive(false);
        hearts.gameObject.SetActive(false);
        HighScoreUpdate();
        backgroundMusic.Pause();
    }

    // void Update()
    // {
        
    // }

    public void RestartGame(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartGame(float difficulty){
        isGameActive = true; 
        score = 0;
        StartCoroutine(SpawnTarget());
        UpdateScore(0);  
        titleScreen.gameObject.SetActive(false); 
        spawnRate /=difficulty;   
        scoreText.gameObject.SetActive(true);
        hearts.gameObject.SetActive(true);
    }

    public void LoseLife()
    {
        health--;
        if (health > 0){
        loseLifeSound.Play();
        }
        if (health == 0)
        {
            health = 0;
            gameOverSound.Play();
            GameOver();
        }
    }

    public void HighScoreUpdate(){
        if (PlayerPrefs.HasKey("HighScore")){
            if (score > PlayerPrefs.GetInt("HighScore")){
                PlayerPrefs.SetInt("HighScore", score);
            }
        }
        else{
            PlayerPrefs.SetInt("HighScore", score);
        }
        finalScoreText.text = "Score: " + score.ToString();
        highScoreText.text = "High Score: " + PlayerPrefs.GetInt("HighScore").ToString();
    }
}
