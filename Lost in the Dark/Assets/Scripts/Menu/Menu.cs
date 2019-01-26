using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {

	private readonly string SCENE = "Game";
    private readonly string THIS = "Main Menu";

    [Tooltip("The different panels in the menu")]
    public GameObject titlePanel;
    public GameObject areYouSurePanel;
    public GameObject mainPanel;
    public GameObject loadingPanel;
    public GameObject soundPanel;
    public GameObject difficultyPanel;

    [Header("SFX")]
	[Tooltip("The GameObject holding the Audio Source component for the SWOOSH SOUND")]
	public GameObject swooshSound;
    public GameObject clickSound;


    private bool asyncReady;
    private bool soundReady;

    void Start() {
        // Clear the player data
        Data.Clear();
        Config.Volume = GetComponent<AudioSource>().volume;
        Config.LevelDifficulty = Difficulty.EASY;
        asyncReady = false;
        soundReady = false;
    }

    public void Play () {
        mainPanel.gameObject.SetActive(false);
        areYouSurePanel.gameObject.SetActive(false);
        soundPanel.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(true);
    }

    public void Back() {
        mainPanel.gameObject.SetActive(true);
        soundPanel.gameObject.SetActive(true);
        difficultyPanel.gameObject.SetActive(false);
    }

    public void Easy() {
        Config.LevelDifficulty = Difficulty.EASY;
        NewGame();
    }

    public void Medium() {
        Config.LevelDifficulty = Difficulty.MEDIUM;
        NewGame();
    }

    public void Hard() {
        Config.LevelDifficulty = Difficulty.HARD;
        NewGame();
    }


    void NewGame() { 
        GetComponent<AudioSource>().Stop();
        Loading();
        StartCoroutine(LoadAsync());
        StartCoroutine(PlaySwoosh());
        StartCoroutine(ChangeWhenReady());
        SceneManager.UnloadSceneAsync(THIS);
    }

    void Loading() {
        titlePanel.gameObject.SetActive(false);
        mainPanel.gameObject.SetActive(false);
        areYouSurePanel.gameObject.SetActive(false);
        soundPanel.gameObject.SetActive(false);
        difficultyPanel.gameObject.SetActive(false);
        loadingPanel.gameObject.SetActive(true);
    }

    IEnumerator ChangeWhenReady() {
        while (!asyncReady || !soundReady) {
            yield return null;
        }
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(SCENE));
    }

    IEnumerator LoadAsync() {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SCENE, LoadSceneMode.Single);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone) {
            yield return null;
        }
        asyncReady = true;
    }

    IEnumerator PlaySwoosh (){
        AudioSource source = swooshSound.GetComponent<AudioSource>();
        source.Play();
        yield return new WaitWhile(() => source.isPlaying);
        soundReady = true;
	}

    public void PlayClick() {
        clickSound.GetComponent<AudioSource>().Play();
    }

    // Are You Sure - Quit Panel Pop Up
    public void AreYouSure (){
        PlayClick();
        soundPanel.gameObject.SetActive(false);
        areYouSurePanel.gameObject.SetActive(true);
	}

	public void No (){
        PlayClick();
        areYouSurePanel.gameObject.SetActive(false);
        soundPanel.gameObject.SetActive(true);
    }

	public void Yes (){
        PlayClick();
        Application.Quit();
	}

    public void IncreaseVolume() {
        if (Config.Volume < 1)
            Config.Volume += 0.1f;
        AdjustSourceVolume();
    }

    public void DecreaseSound() {
        if (Config.Volume > 0)
            Config.Volume -= 0.1f;
        AdjustSourceVolume();
    }

    private void AdjustSourceVolume() {
        GetComponent<AudioSource>().volume = Config.Volume;
    }
}