using UnityEngine;
using TMPro;


public class Timer : MonoBehaviour {
  public float timeRemaining = 60f;
  public TMP_Text timerText; // Default color: 1f, 0.8705883f, 0
  public TMP_Text notifyText;

  private float green = 0.8705883f;
  private int lastSeconds;
  private bool textSizeIncreasing = true;

  public float startingTime;
  private float startingFontSize;
  private float startingGreen;

  [SerializeField] private GameObject endScreen;
  private EndScreen endScreenScript;

  private bool timerPaused = false;
  private bool shouldLevelEnd = true;

  public void resetTimer() {
    timeRemaining = startingTime;
    green = startingGreen;
    timerText.fontSize = startingFontSize;
    timerText.color = new Color(1f, green, 0f);
    timerPaused = false;
    shouldLevelEnd = true;
  }

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

  public void pauseTimer() {
    timerPaused = true;
  }

  void Start() {
    startingTime = timeRemaining;
    startingFontSize = timerText.fontSize;
    startingGreen = green;

    endScreenScript = endScreen.GetComponent<EndScreen>();
  }

  void Update() {
    if (timeRemaining > 0) {
      if (!timerPaused) timeRemaining -= Time.deltaTime;
      displayTime();
      if (checkTick() && timeRemaining < 10) increaseFontIntensity();
    } else {
      // Timer ran out
      // notifyText.text = "Wow you suck!";
      // animateTextSize(notifyText, 102, 97);
      // Activate the end screen
      if (shouldLevelEnd) {
        // TODO: This code is horrendous I'm so ashamed
        endScreenScript.levelOver();
        shouldLevelEnd = false;
      }
    }
  }
}
