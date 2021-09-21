using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GPGSLeaderboard : MonoBehaviour
{

    public Button openLeaderBoard;
    public Button submitScore;
    private PlayGames playGames;

    private void Start()
    {
        openLeaderBoard.onClick.AddListener(OpenLeaderboard);
        submitScore.onClick.AddListener(UpdateLeaderScore);
        playGames = FindObjectOfType<PlayGames>();
    }
    private void OpenLeaderboard()
    {
        Social.ShowLeaderboardUI();
        playGames.ShowAndroidToastMessage("Loading leaderboard...");
    }

    private void UpdateLeaderScore()
    {
      
        Social.ReportScore(PlayerPrefs.GetInt("ClassicHighscore", 1), GPGSIds.leaderboard_classic_high_score, (bool success) =>
          {
              if (success)
              {
                  PlayerPrefs.SetInt("ClassicHighscore", GameManager.singleton.classicModeBestScore);
                  playGames.ShowAndroidToastMessage("Classic High Score succesfully updated to :" + GameManager.singleton.classicModeBestScore);
              }
          });

        Social.ReportScore(PlayerPrefs.GetInt("TimeAttackHighscore", 1), GPGSIds.leaderboard_time_attack_high_score, (bool success) =>
        {
            if (success)
            {
                PlayerPrefs.SetInt("TimeAttackHighscore", GameManager.singleton.timeAttackBestScore);
                playGames.ShowAndroidToastMessage("Time Attack High Score succesfully updated to :" + GameManager.singleton.timeAttackBestScore);
            }
        });

    }

}
