using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour {

    private readonly string SCORE = "SCORE: ";
    private readonly string KILLS = "KILLS: ";
    private readonly string TIME = "YOU SURVIVED: ";
    private readonly string POINTS = " POINTS";
    private readonly string BULLETS = "BULLETS SHOOTED: ";
    private readonly string DAMAGE_RECEIVED = "DAMAGE RECEIVED: ";
    private readonly string DAMAGE_GIVEN = "DAMAGE GIVEN: ";
    private readonly string COLLECTED_ITEMS = "COLLECTED POWER UPS: ";
    private readonly string END_LINE = "\n";
    private readonly string ERROR = "Unable to load your game data";

    private readonly string SCENE = "Main Menu";
    private readonly string THIS = "End Menu";

    public Text text;

    void Start() {
        DisplayPlayerData();
        GetComponent<AudioSource>().volume = Config.Volume;
    }

    public void GoMainMenu() {
        SceneManager.LoadScene(SCENE, LoadSceneMode.Single);
        SceneManager.UnloadSceneAsync(THIS);
    }

    void DisplayPlayerData() {
        try {
            int score = Mathf.RoundToInt(Data.Kills + Data.DamageGiven);
            text.text = SCORE + score + POINTS + END_LINE;
            text.text += KILLS + Data.Kills + END_LINE;
            text.text += TIME + Data.Time + END_LINE;
            text.text += BULLETS + Data.Bullets + END_LINE;
            text.text += DAMAGE_RECEIVED + Data.DamageReceived + END_LINE;
            text.text += DAMAGE_GIVEN + Data.DamageGiven + END_LINE;
            text.text += COLLECTED_ITEMS + Data.Items + END_LINE;
        }catch (UnityException error) {
            text.text = ERROR;
            Debug.Log(error);
        }
    }
}
