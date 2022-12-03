using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour {

  [SerializeField] private RawImage endScreenTrophy;
  [SerializeField] private TMP_Text endScoreText;
  [SerializeField] private TMP_Text endLevelText;
  private Score scoreScript;
  private Timer timerScript;


  public void levelOver() {
    scoreScript = GameObject.FindGameObjectsWithTag("Score")[0].GetComponent<Score>();
    timerScript = FindObjectOfType<Timer>();
    endScreenTrophy.texture = scoreScript.getTrophyTexture();
    endScoreText.text = "Score: $" + scoreScript.getScore();
    timerScript.pauseTimer();
    gameObject.SetActive(true);
  }

  public void levelInProgress() {
    gameObject.SetActive(false);
  }

  public void buttonRestart() {
    // This should be moved to its own script also bugs out the lighting
    SceneManager.LoadScene("Core");
    SceneManager.LoadScene("AudioCore", LoadSceneMode.Additive);
    SceneManager.LoadScene("Game", LoadSceneMode.Additive);
    SceneManager.LoadScene("Game UI", LoadSceneMode.Additive);
    SceneManager.LoadScene("ChinaShop", LoadSceneMode.Additive);
  }

  void Start() {

  }

  void Update() {

  }
}
