using UnityEngine;
using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;


// This scripts handles signin with google play services
public class PlayGames : MonoBehaviour
{

    public Button signOnButton;
    public static PlayGamesPlatform platform;
   

    void Start()
    {
        if (platform == null)
        {
            PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder().Build();
            PlayGamesPlatform.InitializeInstance(config);
            PlayGamesPlatform.DebugLogEnabled = true;
            platform = PlayGamesPlatform.Activate();
        }

       
        CallSignUserIn();
        signOnButton.onClick.AddListener(SignOutUserFromGPGS);
    }

    private void CallSignOut()
    {
        PlayGamesPlatform.Instance.SignOut();
        ShowAndroidToastMessage("You have signed out of Google Play Services");

    }

    public bool CallSignUserIn()
    {
      
        bool isSuccessFul = false;
        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) => {
      
        });

        Social.Active.localUser.Authenticate(success =>
        {
            isSuccessFul = success;
            ShowAndroidToastMessage("You are successfully signed in to Google Play Services");
        });

        return isSuccessFul;

    }

    private void SignOutUserFromGPGS()
    {
        if(signOnButton.GetComponentInChildren<Text>().text == "Sign-In")
        {
            CallSignUserIn();
            signOnButton.GetComponentInChildren<Text>().text = "Sign-Out";
        }
        else
        {
            CallSignOut();
            signOnButton.GetComponentInChildren<Text>().text = "Sign-In";
        }
        
    }


    public void ShowAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }


}