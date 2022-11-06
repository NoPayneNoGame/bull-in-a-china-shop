using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour {
  // I like the idea of centralizing the score text on screen and increasing text size as you do more damage

  public TMP_Text scoreText;

  private int score;

  void OnCollisionEnter(Collision hit) {
    if (hit.gameObject.tag == "Destructible") {
      ScoreValue scoreValue;
      // Since OpenFragments inherit the Destructible tag from their parent we will get an error on collision with them if we try to access the ScoreValue script
      if (scoreValue = hit.gameObject.GetComponent<ScoreValue>()) {
        int objectValue = scoreValue.value;
        scoreValue.setValue(0);
        score += objectValue;
      }
      scoreText.text = "Score: " + score;
    }
  }

}
