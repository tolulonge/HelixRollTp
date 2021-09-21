using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


// Serves as a general class for handling most of the game interaction
public class GameManager : MonoBehaviour
{
    public int classicModeBestScore;
    public int timeAttackBestScore;
    public int clasicScore;
    public int timeAttackScore;

    public int currentStage = 0;

    public AudioClip[] audioClips;
    public static GameManager singleton;

    public Button backButton;
    public Button restartButton;

    public TextMeshProUGUI levelNameTxt;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;


    public Button gameCompletedBtn;
    public TextMeshProUGUI gameCompletedText;


    public TextMeshProUGUI gameOverScoreText;
    public TextMeshProUGUI personalBestScoreRecord;
    private float timeRemaining;
    private bool isTimerRunning = true;

    public bool isGameActive = true;

    public string gameMode;


    // Start is called before the first frame update
    void Awake()
    {
        Advertisement.Initialize("4370173");
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
            Destroy(gameObject);

        classicModeBestScore = PlayerPrefs.GetInt("ClassicHighscore");
        timeAttackBestScore = PlayerPrefs.GetInt("TimeAttackHighscore");
       
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioClips[0]);

        backButton.onClick.AddListener(LoadSelectionMenu);
        restartButton.onClick.AddListener(RestartGame);
        

        if (gameMode == "classic")
        {
            gameCompletedBtn.onClick.AddListener(ResetClassicGame);
            levelNameTxt.text = "LVL : " + (currentStage + 1);
        }
            




        timeRemaining = 20;
        

        if (gameMode == "timed") timerText.gameObject.SetActive(true);

        

    }

    private void Update()
    {
        
        if(gameMode == "timed")
        {
            if (isGameActive)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimer(timeRemaining);
               
            }
        }
       
        updateScore();
    }

    void LoadSelectionMenu()
    {
        Destroy(gameObject);
        Physics.gravity /= 2f;
        SceneManager.LoadScene("SelectOptionScenePortrait");
    }

    public void NextLevel()
    {
        currentStage++;
        FindObjectOfType<BallController>().ResetBall();
        Camera.main.GetComponent<AudioSource>().Stop();
        Camera.main.GetComponent<AudioSource>().PlayOneShot(audioClips[currentStage]);
        FindObjectOfType<HelixController>().LoadStage(currentStage);
        if(gameMode == "classic")
        levelNameTxt.text = "LVL : " + (currentStage + 1);

    }

    public void RestartLevel()
    {

        if(!(gameMode == "timed"))
        {
            singleton.clasicScore = 0;
            restartButton.gameObject.SetActive(true);
            gameOverText.gameObject.SetActive(true);
        }

        singleton.timeAttackScore = 0;

        FindObjectOfType<BallController>().ResetBall();
        FindObjectOfType<HelixController>().LoadStage(currentStage);
      


        // Reload stage
    }

    public void AddScore(int scoreToAdd)
    {
        if(gameMode == "classic" && isGameActive)
        {
            clasicScore += scoreToAdd;

            if (clasicScore > classicModeBestScore)
            {
                classicModeBestScore = clasicScore;
                // Store highScore in PlayerPrefs
                PlayerPrefs.SetInt("ClassicHighscore", clasicScore);
                UpdatePersonalBest();

            }
        }
        else if(gameMode == "timed" && isGameActive)
        {
            timeAttackScore += scoreToAdd;

            if (timeAttackScore > timeAttackBestScore)
            {
                timeAttackBestScore = timeAttackScore;
                // Store highScore in PlayerPrefs
                PlayerPrefs.SetInt("TimeAttackHighscore", timeAttackScore);
                UpdatePersonalBest();

            }
        }
        

       
    }

    private void UpdateTimer(float timeRemaining)
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                float seconds = Mathf.FloorToInt(timeRemaining % 60);
                timerText.text = "T I M E : " + seconds;
            }
            else
            {

                GameOver();
                timeRemaining = 0;
                isTimerRunning = false;
            }
        }

    }


    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        isGameActive = false;
        restartButton.gameObject.SetActive(true);
        gameOverScoreText.gameObject.SetActive(true);

        if (gameMode == "classic")
            gameOverScoreText.text = "" + clasicScore;
        else gameOverScoreText.text = "" + timeAttackScore;
        
    }

    void updateScore()
    {
        if(gameMode == "classic")
        {
            bestScoreText.text = "B E S T : " + classicModeBestScore;
            scoreText.text = "S C O R E : " + clasicScore;
        }
        else
        {
            bestScoreText.text = "B E S T : " + timeAttackBestScore;
            scoreText.text = "S C O R E : " + timeAttackScore;
        }
        
    }

    private void RestartGame()
    {
        if(gameMode == "classic")
        {
            Advertisement.Show();
        }
        
        gameOverScoreText.gameObject.SetActive(false);
        personalBestScoreRecord.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        timeRemaining = 20;
        restartButton.gameObject.SetActive(false);
        isGameActive = true;
        if(gameMode == "timed")
        isTimerRunning = true;
        singleton.clasicScore = 0;
        singleton.timeAttackScore = 0;
        FindObjectOfType<BallController>().ResetBall();
        if(gameMode == "classic")
        {
          FindObjectOfType<HelixController>().LoadStage(currentStage);
        }else
        FindObjectOfType<HelixController>().LoadStage(0);
    }

    public void GameCompleted()
    {
        if(gameMode == "classic")
        {
            gameCompletedText.gameObject.SetActive(true);
            gameCompletedBtn.gameObject.SetActive(true);
            isGameActive = false;
        }

    }

    private void ResetClassicGame()
    {
        if (gameMode == "classic")
        {
            gameCompletedText.gameObject.SetActive(false);
            gameCompletedBtn.gameObject.SetActive(false);
            isGameActive = true;

            SceneManager.LoadScene("ClassicModePortrait");
            Physics.gravity /= 2f;
            levelNameTxt.text = "LVL : " + (currentStage + 1);
        }
        
    }

    private void UpdatePersonalBest()
    {
        if(gameMode == "classic")
        {
            personalBestScoreRecord.gameObject.SetActive(true);


            personalBestScoreRecord.text = "New Personal Best: " + classicModeBestScore;
        }
        else
        {
            personalBestScoreRecord.gameObject.SetActive(true);


            personalBestScoreRecord.text = "New Personal Best: " + timeAttackBestScore;
        }
        
    }
}
