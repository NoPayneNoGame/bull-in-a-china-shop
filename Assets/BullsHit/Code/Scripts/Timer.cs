using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour {
  public float timeRemaining = 60f;
  public TMP_Text timerText;

  void displayTime(float timeF) {
    float minutes = Mathf.FloorToInt(timeF / 60);
    float seconds = Mathf.FloorToInt(timeF % 60);
    timerText.text = minutes + ":" + seconds;
  }

  void Update() {
    if (timeRemaining > 0) {
      timeRemaining -= Time.deltaTime;
      displayTime(timeRemaining);
    } else {
      // Timer ran out
    }
  }
}
