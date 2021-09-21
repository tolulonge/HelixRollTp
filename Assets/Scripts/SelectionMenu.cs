using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using GooglePlayGames;


/*
 * This is the selection menu used for the game, this scripts handles loading of
 * the classic and time attack mode, hiding buttons and playing sounds when 
 * button is clicked
 */
public class SelectionMenu : MonoBehaviour
{
    public Button classic;
    public Button timed;
    public Button quit;
    private AudioSource audioSource;
    public AudioClip selectedClip;



    public TextMeshProUGUI loadingText;

 
    // Start is called before the first frame update
    void Start()
    {
        classic.onClick.AddListener(Classic);
        timed.onClick.AddListener(Timed);
        quit.onClick.AddListener(Quit);
        audioSource = GetComponent<AudioSource>();
    }

   private void Classic()
    {
        SceneManager.LoadScene("ClassicModePortrait");
        loadingText.gameObject.SetActive(true);
        audioSource.PlayOneShot(selectedClip);
        DisableButtons();
    }

    private void Timed()
    {
        SceneManager.LoadScene("TImedModePortrait");
        loadingText.gameObject.SetActive(true);
        audioSource.PlayOneShot(selectedClip);
        DisableButtons();

    }

    private void Quit()
    {
        audioSource.PlayOneShot(selectedClip);
        DisableButtons();
        Application.Quit();
        //to quit out of the editor
       // UnityEditor.EditorApplication.isPlaying = false;

    }

    private void DisableButtons()
    {
        classic.gameObject.SetActive(false);
        timed.gameObject.SetActive(false);
        quit.gameObject.SetActive(false);
    }
}
