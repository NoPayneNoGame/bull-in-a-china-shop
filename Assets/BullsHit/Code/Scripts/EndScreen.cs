using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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

  void Start() {

  }

  void Update() {

  }
}
