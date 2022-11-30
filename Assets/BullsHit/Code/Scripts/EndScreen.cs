using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class EndScreen : MonoBehaviour {

  [SerializeField] private RawImage endScreenTrophy;
  [SerializeField] private TMP_Text endScoreText;
  [SerializeField] private TMP_Text endLevelText;
  [SerializeField] private GameObject winParticles;
  [SerializeField] private GameObject loseParticles;
  private Score scoreScript;
  private Timer timerScript;

  private PlayerMovement playerMovement;

  public void levelOver() {
    playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    playerMovement.enabled = false;

    scoreScript = GameObject.FindGameObjectsWithTag("Score")[0].GetComponent<Score>();
    timerScript = FindObjectOfType<Timer>();

    endScreenTrophy.texture = scoreScript.getTrophyTexture();
    endScoreText.text = "Score: $" + scoreScript.getScore();

    if (scoreScript.getTrophy() == Score.Trophy.none) {
      loseParticles.SetActive(true);
    } else {
      winParticles.SetActive(true);
    }

    disableHud();
    timerScript.pauseTimer();
    gameObject.SetActive(true);
  }

  public void levelInProgress() {
    gameObject.SetActive(false);
  }

  public void buttonRestart() {
    disableEndGame();
    reset();
    SceneController.instance.reloadLevel();
  }

  public void buttonHome() {
    disableEndGame();
    reset();
    SceneController.instance.loadMainMenu();
  }

  void reset() {
    playerMovement.enabled = true;
    scoreScript.resetScore();
    timerScript.resetTimer();
  }

  void disableHud() {
    GameObject.FindGameObjectWithTag("HUD").SetActive(false);
  }

  void disableEndGame() {
    Debug.Log(GameObject.FindGameObjectWithTag("EndGame"));
    GameObject.FindGameObjectWithTag("EndGame").SetActive(false);
  }
}
