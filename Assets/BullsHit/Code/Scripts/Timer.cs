using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour {
  public float timeRemaining = 60f;
  public TMP_Text timerText; // Default color: 1f, 0.8705883f, 0
  public TMP_Text notifyText;

  private float green = 0.8705883f;
  private int lastSeconds;
  private bool textSizeIncreasing = true;

  void displayTime() {
    float time = timeRemaining + 1;
    float minutes = Mathf.FloorToInt(time / 60);
    float seconds = Mathf.FloorToInt(time % 60);
    timerText.text = minutes + ":" + seconds.ToString("00");
  }

  void increaseFontIntensity() {
    // TODO: Move to its own script, this is also used in Score.cs
    green -= 0.1f;
    timerText.color = new Color(1f, green, 0f);
    timerText.fontSize += 6;
  }

  bool checkTick() {
    int current = Mathf.FloorToInt(timeRemaining);
    if (current != lastSeconds) {
      lastSeconds = current;
      return true;
    }
    return false;
  }

  void animateTextSize(TMP_Text text, int maxSize, int minSize) {
    if (text.fontSize >= maxSize) {
      textSizeIncreasing = false;
    }
    if (text.fontSize <= minSize) {
      textSizeIncreasing = true;
    }
    if (textSizeIncreasing) {
      text.fontSize += 0.1f;
    } else {
      text.fontSize -= 0.1f;
    }
  }

  void Update() {
    if (timeRemaining > 0) {
      timeRemaining -= Time.deltaTime;
      displayTime();
      if (checkTick() && timeRemaining < 10) increaseFontIntensity();
    } else {
      // Timer ran out
      notifyText.text = "Wow you suck!";
      animateTextSize(notifyText, 102, 97);
    }
  }
}
